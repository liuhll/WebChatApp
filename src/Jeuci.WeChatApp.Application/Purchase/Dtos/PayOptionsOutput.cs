using Abp.AutoMapper;
using Jeuci.WeChatApp.Pay.Models;

namespace Jeuci.WeChatApp.Purchase.Dtos
{

    [AutoMap(typeof(PayOptions))]
    public class PayOptionsOutput
    {
        public string Timestamp { get; set; }

        public string NonceStr { get; set; }

        public string Package { get; set; }

        public string PaySign { get; set; }
    }
}