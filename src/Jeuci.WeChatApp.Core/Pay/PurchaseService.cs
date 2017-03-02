using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Repositories;
using Abp.Logging;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Exceptions;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Pay.Lib;
using Jeuci.WeChatApp.Pay.Models;
using Jeuci.WeChatApp.Pay.Tool;
using Jeuci.WeChatApp.Repositories;
using Jeuci.WeChatApp.Wechat.Authentication;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.Pay
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IWechatAuthentManager _wechatAuthentManager;

        private readonly IPurchaseServiceRepository _purchaseServiceRepository;

        private readonly IRepository<UserPayOrderInfo,string> _userPayOrdeRepository;

        private readonly IOrderPolicy _orderPolicy;

        public PurchaseService(IWechatAuthentManager wechatAuthentManager,
            IPurchaseServiceRepository purchaseServiceRepository, IRepository<UserPayOrderInfo,string> userPayOrdeRepository, IOrderPolicy orderPolicy)
        {
            _wechatAuthentManager = wechatAuthentManager;
            _purchaseServiceRepository = purchaseServiceRepository;
            _userPayOrdeRepository = userPayOrdeRepository;
            _orderPolicy = orderPolicy;
        }

        public WxPayData UnifiedOrderResult(ServiceOrder serviceOrder ,int timeOut = 10)
        {

            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";

            //1. 检测必填参数
            CheckParams(serviceOrder);

            WxPayData wxPayData = new WxPayData();

            // 2.通过反射设置参数值
            var dataProperties = serviceOrder.GetType().GetProperties();

            foreach (var property in dataProperties)
            {
                if (property.GetValue(serviceOrder, null) != null)
                {
                    wxPayData.SetValue(property.Name, property.GetValue(serviceOrder, null));
                }
                
            }

           // 3.签名
            wxPayData.SetValue("sign",wxPayData.MakeSign());

            //4.获取上传的xml字符串参数
            string xml = wxPayData.ToXml();

            try
            {
                LogHelper.Logger.Debug("UnfiedOrder request : " + xml);
                string response = HttpService.Post(xml,url, false, timeOut);
                LogHelper.Logger.Debug("UnfiedOrder response : " + response);

                WxPayData result = new WxPayData();
                result.FromXml(response);

                if (result.GetValue("return_code").ToString() == "FAIL")
                {
                    LogHelper.Logger.Error("调用统一下单接口失败");
                    throw new OrderException("下单失败，请稍后重试!");
                }

                result.SetValue("orderid", wxPayData.GetValue("out_trade_no"));

                return result;
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("调用统一下单接口失败,原因:"+e.Message);
                throw new OrderException("由于网络原因下单失败,请重试");
            }


        }


        public WxpayOptions GetWxpayOptions(string nonceStr,string url)
        {
          //  var jsapiTicket = _wechatAuthentManager.GetJsapiTicket();
            var appId = _wechatAuthentManager.AppId;
            var timestamp = DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString();
            var signStr = GetSignStr(appId,nonceStr,url,timestamp);
            return new WxpayOptions()
            {
                AppId = appId,
                NonceStr = nonceStr,
                Signature = signStr,
                Timestamp = timestamp,
                Url = url,
                JsapiTicket = _wechatAuthentManager.GetJsapiTicket()

            };

        }

        public PayOptions GetPaySign(string package, string nonceStr)
        {
            var appId = _wechatAuthentManager.AppId;
            var timestamp = DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString();

            WxPayData data = new WxPayData();
            data.SetValue("appId", appId);
            data.SetValue("timeStamp", timestamp);
            data.SetValue("nonceStr", nonceStr);
            data.SetValue("signType", "MD5");
            data.SetValue("package", package);

            var encryStr = data.MakeSign();

           // LogHelper.Logger.Info("支付API接口加密后的串为:" + encryStr);

            return new PayOptions()
            {
                Package = package,
                NonceStr = nonceStr,
                PaySign = encryStr,
                Timestamp = timestamp,
            };
        }

        public bool GeneratePayOrder(PayOrder payOrder, out string msg)
        {
            var orderInfo = _userPayOrdeRepository.FirstOrDefault(p => p.Id == payOrder.ID);
            if (orderInfo != null)
            {
                if (orderInfo.State <= 1)
                {
                    msg = "Success";
                    return true;
                }
                msg = "您已经支付过该订单，请不要重复支付!";
                return false;
            }
            msg = "Success";
            return _purchaseServiceRepository.GeneratePayOrder(payOrder);
        }

        public int CompleteServiceOrder(WxPayData queryData)
        {
            string out_trade_no = queryData.GetValue("out_trade_no").ToString().Substring(10);
            //取出提交的数据包，原样返回
            //object attachData = payData.GetValue("attach");
            //交易状态
            string trade_state = queryData.GetValue("trade_state").ToString();
            //微信支付订单号
            string transaction_id = null;
            if (queryData.IsSet("transaction_id"))
                transaction_id = queryData.GetValue("transaction_id").ToString();

            var orderInfo = _userPayOrdeRepository.FirstOrDefault(p => p.Id == out_trade_no);

            if (trade_state == "SUCCESS")//交易成功
            {
                //支付金额
                return _purchaseServiceRepository.CompleteServiceOrder(new CompleteServiceOrder()
                {
                    OrderID = out_trade_no,
                    Cost = Convert.ToDouble(queryData.GetValue("total_fee")) / 100,
                    NewID = OrderHelper.GenerateNewId(),
                    PayExtendInfo = queryData.ToJson(),
                    PayState = trade_state,
                    PayOrderID = transaction_id,
                    Remarks = "微信公众号支付",
                });
            }

            else if (trade_state == "USERPAYING")//正在支付
            {

                var result = _purchaseServiceRepository.UpdateServiceOrder(new UpdateServiceOrder()
                {
                    ID = out_trade_no,
                    PayExtendInfo = queryData.ToJson(),
                    PayState = trade_state,
                    PayOrderID = transaction_id,
                    State = null
                });

                if (result != 0)
                {
                    LogHelper.Logger.Error("更新订单失败");
                }

                return -4;
            }
            else if (trade_state == "NOTPAY")
            {
                //算作超时关闭订单
                if (orderInfo != null && orderInfo.CreateTime.AddMinutes(20) < DateTime.Now)
                {
                    var result1 = _purchaseServiceRepository.UpdateServiceOrder(new UpdateServiceOrder()
                    {
                        ID = out_trade_no,
                        PayExtendInfo = queryData.ToJson(),
                        PayState = trade_state,
                        PayOrderID = transaction_id,
                        State = 3
                    });

                    if (result1 != 0)
                    {
                        LogHelper.Logger.Error("更新订单失败");
                    }
                    LogHelper.Logger.Debug("超时关闭订单");
                    return -4;
                }
                else
                {
                    //继续等待，还不算结束
                    var result1 = _purchaseServiceRepository.UpdateServiceOrder(new UpdateServiceOrder()
                    {
                        ID = out_trade_no,
                        PayExtendInfo = queryData.ToJson(),
                        PayState = trade_state,
                        PayOrderID = transaction_id,
                        State = null
                    });

                    return -4;

                }


            }
            else
            {
                var result1 = _purchaseServiceRepository.UpdateServiceOrder(new UpdateServiceOrder()
                {
                    ID = out_trade_no,
                    PayExtendInfo = queryData.ToJson(),
                    PayState = trade_state,
                    PayOrderID = transaction_id,
                    State = 3
                });

                if (result1 != 0)
                {
                    LogHelper.Logger.Error("更新订单失败");
                }

                LogHelper.Logger.Error("支付失败");

                return -4;
            }

           
        }

        public void FailServiceOrder(UpdateServiceOrder order)
        {
            LogHelper.Logger.Info(JsonHelper.ToJson(order));
            _purchaseServiceRepository.UpdateServiceOrder(order);
        }

        public int ClientCompleteServiceOrder(CompleteServiceOrder order)
        {
            LogHelper.Logger.Debug("订单信息:"+order.ToJson());
            var orderId = order.OrderID;
            var orderInfo = _userPayOrdeRepository.FirstOrDefault(p => p.Id == orderId);
            if (orderInfo == null)
            {
                return -2;
            }

            LogHelper.Logger.Debug("查询到的订单信息为：" + orderInfo.ToJson());

            if (orderInfo.State == 2)
            {
                return 0;
            }

          
            var query = _orderPolicy.Orderquery(string.Format("{0}{1}",WxPayConfig.MCHID, order.OrderID),OrderType.OutTradeNo);
           
            if (query == null)
            {
                LogHelper.Logger.Error("没有查询到相关的订单信息");
                _purchaseServiceRepository.UpdateServiceOrder(new UpdateServiceOrder()
                {
                    ID = orderId,
                    State = 3,
                    PayState = "FAIL",
                });
                return -1;
            }
            else
            {
                LogHelper.Logger.Debug(query.ToJson());
                if (query.IsSet("trade_state") && query.GetValue("trade_state").ToString() == "SUCCESS")
                {
                   var orderState = _purchaseServiceRepository.CompleteServiceOrder(new CompleteServiceOrder()
                    {
                        OrderID = orderId,
                        Cost = (double) Convert.ToInt32(query.GetValue("total_fee")) / 100,
                        NewID = OrderHelper.GenerateNewId(),
                        PayState = query.GetValue("trade_state").ToString(),
                        PayExtendInfo = query.ToXml(),
                        PayOrderID = query.GetValue("transaction_id").ToString(),
                        Remarks = "微信公众号支付"
                    });

                    return orderState;
                }
                else
                {
                    return -1;
                }
            }

        }

        public IList<UserPayOrderInfo> GetNeedQueryOrderList(PayMode mobileWeb)
        {
            return _purchaseServiceRepository.GetNeedQueryOrderList(mobileWeb);
        }

        private string GetSignStr(string appId, string nonceStr, string url, string timestamp)
        {
            var dict = new SortedDictionary<string,string>();
            dict.Add("noncestr",nonceStr);
            dict.Add("jsapi_ticket",_wechatAuthentManager.GetJsapiTicket());
            dict.Add("timestamp",timestamp);
            dict.Add("url",url);
            var line = new StringBuilder();
            foreach (var item in dict)
            {
                line.Append(item.Key != "url"
                    ? string.Format("{0}={1}&", item.Key, item.Value)
                    : string.Format("{0}={1}", item.Key, item.Value));
            }

            var line1 = line.ToString();

            var encryStr = EncryptionHelper.EncryptToSHA1(line1);
            LogHelper.Logger.Info("统一下单API接口需要加密的串：" + line1);
            LogHelper.Logger.Info("统一下单API接口加密后的串为:" + encryStr);

            return encryStr;

        }

        private void CheckParams(ServiceOrder serviceOrder)
        {
            //检测必填参数
            if (string.IsNullOrEmpty(serviceOrder.out_trade_no))
            {
                throw new WxPayException("缺少统一支付接口必填参数out_trade_no！");
            }
            else if (string.IsNullOrEmpty(serviceOrder.body))
            {
                throw new WxPayException("缺少统一支付接口必填参数body！");
            }
            else if (serviceOrder.total_fee <= 0)
            {
                throw new WxPayException("缺少统一支付接口必填参数total_fee！");
            }
            else if (string.IsNullOrEmpty(serviceOrder.trade_type))
            {
                throw new WxPayException("缺少统一支付接口必填参数trade_type！");
            }

            //关联参数
            if (serviceOrder.trade_type == "JSAPI" && string.IsNullOrEmpty(serviceOrder.openid))
            {
                throw new WxPayException("统一支付接口中，缺少必填参数openid！trade_type为JSAPI时，openid为必填参数！");
            }
            if (serviceOrder.trade_type == "NATIVE" && string.IsNullOrEmpty(serviceOrder.product_id))
            {
                throw new WxPayException("统一支付接口中，缺少必填参数product_id！trade_type为JSAPI时，product_id为必填参数！");
            }
        }

       
    }
}