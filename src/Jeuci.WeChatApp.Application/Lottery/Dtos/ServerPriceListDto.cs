using Abp.AutoMapper;
using Jeuci.WeChatApp.Lottery.Models;
using Jeuci.WeChatApp.Wechat.Models.BindEmail;

namespace Jeuci.WeChatApp.Lottery.Dtos
{
    [AutoMap(typeof(ServerPrice))]
    public class ServerPriceListDto
    {
        public int Id { get; set; }

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