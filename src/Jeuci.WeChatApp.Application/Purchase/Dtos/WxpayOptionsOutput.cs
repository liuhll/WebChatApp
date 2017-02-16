using Abp.AutoMapper;
using Jeuci.WeChatApp.Pay.Models;

namespace Jeuci.WeChatApp.Purchase.Dtos
{
    [AutoMap(typeof(WxpayOptions))]
    public class WxpayOptionsOutput
    {
        public string AppId { get; set; }

        public string Timestamp { get; set; }

        public string NonceStr { get; set; }

        public string Signature { get; set; }

        //public string Url { get; set; }

        //public string JsapiTicket { get; set; }
    }
}