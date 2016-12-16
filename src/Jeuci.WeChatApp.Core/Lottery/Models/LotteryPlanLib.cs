using System;
using Abp.Domain.Entities;

namespace Jeuci.WeChatApp.Lottery.Models
{
    public class LotteryPlanLib : Entity<string>
    {

        public int SId { get; set; }

        public int State { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string VCode { get; set; }
    }
}