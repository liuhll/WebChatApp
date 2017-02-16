using System.Collections.Generic;
using Abp.AutoMapper;
using Jeuci.WeChatApp.Lottery.Models;

namespace Jeuci.WeChatApp.Lottery.Dtos
{
    [AutoMap(typeof(ServerPriceInfo))]
    public class ServerInfoDto
    {
        public string ServiceName { get; set; }

        public IList<ServerPriceListDto> ServerPrices { get; set; }

        public IList<string> DescriptionList { get; set; }

    }
}