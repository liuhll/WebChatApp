using System.Collections.Generic;

namespace Jeuci.WeChatApp.Lottery.Models
{
    public class ServerList
    {
        public string CPType { get; set; }

        public List<CPNames> CPNames { get; set; }
    }
}