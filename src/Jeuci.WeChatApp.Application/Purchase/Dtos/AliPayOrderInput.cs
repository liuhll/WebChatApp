using Abp.AutoMapper;
using Jeuci.WeChatApp.Pay.Models;

namespace Jeuci.WeChatApp.Purchase.Dtos
{
    [AutoMap(typeof(AliPayOrder))]
    public class AliPayOrderInput : PayOrderInputBase
    {
        public int Uid { get; set; }
    }
}