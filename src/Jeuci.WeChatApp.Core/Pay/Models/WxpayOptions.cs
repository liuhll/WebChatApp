namespace Jeuci.WeChatApp.Pay.Models
{
    public class WxpayOptions
    {
        public string AppId { get; set; }

        public string Timestamp { get; set; }

        public string NonceStr { get; set; }

        public string Signature { get; set; }

        public string Url { get; set; }

        public string JsapiTicket { get; set; }
    }
}