using Abp.Domain.Entities;

namespace Jeuci.WeChatApp.Lottery.Models
{
    public class  ServerPrice : Entity
    {
        public int ServiceId { get; set; }

        public decimal Price { get; set; }

        public decimal AgentPrice { get; set; }

        public decimal BeforeDiscountPrice { get; set; }

        public string AuthDesc { get; set; }

        public int State { get; set; }

        public int CanByOnline { get; set; }

        public string Description { get; set; }
    }
}