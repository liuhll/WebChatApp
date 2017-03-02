using System;
using Abp.Logging;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Pay.Lib;
using Jeuci.WeChatApp.Pay.Models;

namespace Jeuci.WeChatApp.Pay.AliPay
{
    public class AlipayRequest : IAlipayRequest
    {
        private readonly IAopClient _aopClient;

        public AlipayRequest()
        {
            _aopClient = new DefaultAopClient(AliPayConfig.SERVER_URL, AliPayConfig.APPID, AliPayConfig.APP_PRIVATE_KEY, AliPayConfig.FORMAT, AliPayConfig.CHARSET, AliPayConfig.SIGN_TYPE, AliPayConfig.ALIPAY_PUBLIC_KEY);
        }

        public bool Wappay(AlipayOrderOptions options,out string msg)
        {
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.open.public.template.message.industry.modify 
            AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
            //SDK已经封装掉了公共参数，这里只需要传入业务参数
            request.SetNotifyUrl(AliPayConfig.NOTIFY_URL);
            request.SetReturnUrl(AliPayConfig.RETURN_URL);
            LogHelper.Logger.Debug(AliPayConfig.NOTIFY_URL);
            request.BizContent = options.ToJson();
            string form = _aopClient.pageExecute(request).Body;
            msg = form;
            return true;
        }

        public AlipayData Query(string id, OrderType outTradeNo)
        {
            AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
            var requstData = "{\"" + (outTradeNo == OrderType.OutTradeNo ? "out_trade_no" : "trade_no") + "\":\"" + id + "\"}";

                //string.Format(, outTradeNo == OrderType.OutTradeNo ? "out_trade_no" : "trade_no",id);
            request.BizContent = requstData;
            var response = _aopClient.Execute(request);
            var alipayData =  new AlipayData();
            if (response.IsError)
            {
                LogHelper.Logger.Error("查询订单失败：" + response.Msg + "," + response.SubMsg);
                if (response.Code == "40004")
                {
                    alipayData.SetValue("trade_status","NOPAY");
                    alipayData.SetValue(outTradeNo == OrderType.OutTradeNo ? "out_trade_no" : "trade_no",id);
                    return alipayData;
                }

                throw new Exception("查询订单失败，原因" + response.Msg);
            }
            alipayData.SetValue("trade_status",response.TradeStatus);
            alipayData.SetValue("trade_no", response.TradeStatus);
            alipayData.SetValue("out_trade_no", response.OutTradeNo);
            alipayData.SetValue("buyer_logon_id", response.BuyerLogonId);
            alipayData.SetValue("total_amount", response.TotalAmount);
            alipayData.SetValue("receipt_amount", response.ReceiptAmount);
            alipayData.SetValue("buyer_pay_amount", response.BuyerPayAmount);
            return alipayData;
        }
    }
}