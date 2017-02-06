using System.Collections.Generic;

namespace Jeuci.WeChatApp.Lottery.Models
{
    public class ServerPriceInfo
    {
        public string ServiceName { get; set; }

        public IList<ServerPrice> ServerPrices { get; set; }
    }
}