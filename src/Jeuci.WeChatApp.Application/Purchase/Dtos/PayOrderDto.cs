using Abp.AutoMapper;
using Jeuci.WeChatApp.Pay.Models;

namespace Jeuci.WeChatApp.Purchase.Dtos
{
    [AutoMap(typeof(CompleteServiceOrder))]
    public class PayOrderDto
    {
        public string OrderID { get; set; }

        public double Cost { get; set; }

        public string NewID { get; set; }

        public string Remarks { get; set; }

        public string PayOrderID { get; set; }

        public string PayExtendInfo { get; set; }

        public string PayState { get; set; }

        public string PrepayId { get; set; }
    }
}