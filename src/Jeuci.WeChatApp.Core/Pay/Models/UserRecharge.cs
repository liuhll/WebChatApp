using System;
using Abp.Domain.Entities;

namespace Jeuci.WeChatApp.Pay.Models
{
    public class UserRecharge :Entity<string>
    {
        public int UId { get; set; }

        public int? AdminId { get; set; }

        public decimal Cost { get; set; }

        public DateTime CreateTime { get; set; }

        public string OrderID { get; set; }

        public string Remarks { get; set; }
    }
}