using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using Abp.Logging;
using Abp.WebApi.Controllers;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Pay;
using Jeuci.WeChatApp.Pay.Models;
using Jeuci.WeChatApp.Pay.Tool;
using Jeuci.WeChatApp.Purchase;
using Jeuci.WeChatApp.Purchase.Dtos;

namespace Jeuci.WeChatApp.Api.Controllers
{
    public class PayController : AbpApiController
    {
        private readonly IPurchaseAppService _purchaseAppService;

        private readonly IOrderPolicy _orderPolicy;

        public PayController(IPurchaseAppService purchaseAppService, IOrderPolicy orderPolicy)
        {
            _purchaseAppService = purchaseAppService;
            _orderPolicy = orderPolicy;
        }

        public HttpResponseMessage PurchaseService()
        {
            LogHelper.Logger.Debug("开始回调支付接口");
            Senparc.Weixin.MP.TenPayLibV3.ResponseHandler payNotifyRepHandler = new Senparc.Weixin.MP.TenPayLibV3.ResponseHandler(HttpContext.Current);
            payNotifyRepHandler.SetKey(WxPayConfig.KEY);
            string return_code = payNotifyRepHandler.GetParameter("return_code");//返回状态码
            string return_msg = payNotifyRepHandler.GetParameter("return_msg");//返回信息

            string xml = string.Format(@"", return_code, return_msg);
            var res = Request.CreateResponse(HttpStatusCode.OK);

            // 通信失败
            if (return_code.ToUpper() != "SUCCESS")
            {
                //支付失败调用
                if (string.IsNullOrEmpty(payNotifyRepHandler.GetParameter("out_trade_no")))
                {
                    _purchaseAppService.FailServiceOrder(new UpdateServiceOrder()
                    {
                        ID = payNotifyRepHandler.GetParameter("out_trade_no").Substring(10),
                        PayState = payNotifyRepHandler.GetParameter("return_code"),
                        PayExtendInfo = payNotifyRepHandler.ParseXML(),
                        State = 3,
                    });
                }
                xml = "<xml>" +
               "<return_code><![CDATA[FAIL]]></return_code>" +
               "<return_msg><![CDATA[Fail]]></return_msg>" +
               "</xml>";
                Logger.Error("交易失败");
                res.Content = new StringContent(xml, Encoding.UTF8, "text/xml");
                return res;
            }

            if (!payNotifyRepHandler.IsTenpaySign())
            {
                xml = "<xml>" +
                    "<return_code><![CDATA[FAIL]]></return_code>" +
                    "<return_msg><![CDATA[sign is error]]></return_msg>" +
                    "</xml>";

                LogHelper.Logger.Debug("签名验证未通过");
                res.Content = new StringContent(xml, Encoding.UTF8, "text/xml");
                return res;
            }

            LogHelper.Logger.Debug(payNotifyRepHandler.GetParameter("out_trade_no").Substring(10) + "支付并且验证成功！");

            var transactionId = payNotifyRepHandler.GetParameter("transaction_id");

            if (string.IsNullOrEmpty(transactionId))
            {
                xml = "<xml>" +
                    "<return_code><![CDATA[FAIL]]></return_code>" +
                    "<return_msg><![CDATA[微信支付单不存在]]></return_msg>" +
                    "</xml>";
                LogHelper.Logger.Error("微信支付单不存在");
                res.Content = new StringContent(xml, Encoding.UTF8, "text/xml");
                return res;
            }

            var payData = _orderPolicy.Orderquery(transactionId, OrderType.TransactionId);

            Logger.Debug(payData.ToJson());

            if (payData.GetValue("return_code").ToString() != "SUCCESS" ||
                payData.GetValue("result_code").ToString() != "SUCCESS")
            {
                //订单查询失败，则立即返回结果给微信支付后台
                xml = "<xml>" +
                      "<return_code><![CDATA[FAIL]]></return_code>" +
                      "<return_msg><![CDATA[订单查询失败]]></return_msg>" +
                      "</xml>";
                LogHelper.Logger.Error("订单查询失败");
                res.Content = new StringContent(xml, Encoding.UTF8, "text/xml");
                return res;
            }

            var result = _purchaseAppService.CompleteServiceOrder(payData);
                if (result.Code == ResultCode.Success)
                {
                    xml = "<xml>" +
                          "<return_code><![CDATA[SUCCESS]]></return_code>" +
                          "<return_msg><![CDATA[OK]]></return_msg>" +
                          "</xml>";

                    LogHelper.Logger.Debug("交易成功！");
                }
                else
                {
                    xml = "<xml>" +
                          "<return_code><![CDATA[FAIL]]></return_code>" +
                          "<return_msg><![CDATA[" + result.Msg + "]]></return_msg>" +
                          "</xml>";

                    LogHelper.Logger.Debug(string.Format("{0}！订单号为{1},交易单号为：{2}",result.Msg,
                        payNotifyRepHandler.GetParameter("out_trade_no").Substring(10), payNotifyRepHandler.GetParameter("transaction_id")));

                }

                res.Content = new StringContent(xml, Encoding.UTF8, "text/xml");
                LogHelper.Logger.Debug(return_code + return_msg);

                return res;
            }
        }
    }