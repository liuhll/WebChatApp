using Abp.AutoMapper;

namespace Jeuci.WeChatApp.Pay.Models
{
    public class PayOptions
    {
        public string Timestamp { get; set; }

        public string NonceStr { get; set; }

        public string Package { get; set; }

        public string PaySign { get; set; }
    }
}