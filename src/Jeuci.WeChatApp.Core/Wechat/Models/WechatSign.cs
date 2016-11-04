using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeuci.WeChatApp.Common.Tools;

namespace Jeuci.WeChatApp.Wechat.Models
{
    public class WechatSign
    {
        public string Signature { get; set; }

        public long Timestamp { get; set; }

        public int Nonce { get; set; }

        public string Echostr { get; set; }
       
    }
}
