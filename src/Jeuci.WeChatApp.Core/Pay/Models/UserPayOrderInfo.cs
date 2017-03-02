using System;
using Abp.Domain.Entities;

namespace Jeuci.WeChatApp.Pay.Models
{
    public class UserPayOrderInfo: Entity<string>
    {
        public int UId { get; set; }

        public int? State { get; set; }

        public DateTime CreateTime { get; set; }

        public string PayAppID { get; set; }

        public int GoodsType { get; set; }

        public string PayExtendInfo { get; set; }

        public string PayState { get; set; }

        public string PayOrderID { get; set; }

        public DateTime? UpdateTime { get; set; }

        public decimal Cost { get; set; }

        public int PayType { get; set; }
    }
}