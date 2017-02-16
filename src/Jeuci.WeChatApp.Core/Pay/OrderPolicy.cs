using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Pay.Lib;

namespace Jeuci.WeChatApp.Pay
{
    public class OrderPolicy : IOrderPolicy
    {
         

        public WxPayData Orderquery(string orderId, OrderType orderType)
        {
            string queryApiAddress = "https://api.mch.weixin.qq.com/pay/orderquery";
            var wxPayData = new WxPayData();
            wxPayData.SetValue("appid",WxPayConfig.APPID);
            wxPayData.SetValue("mch_id",WxPayConfig.MCHID);
            wxPayData.SetValue(orderType == OrderType.OutTradeNo 
                ? "out_trade_no" : "transaction_id", orderId);

            wxPayData.SetValue("nonce_str",RandomHelper.GenerateNonce());
            wxPayData.SetValue("sign",wxPayData.MakeSign());

            string xml = wxPayData.ToXml();
            var response = HttpService.Post(xml, queryApiAddress, false, 10);
            if (string.IsNullOrEmpty(response))
            {
                return null;
            }
            WxPayData result = new WxPayData();
            result.FromXml(response);

            return result;
        }
    }
}