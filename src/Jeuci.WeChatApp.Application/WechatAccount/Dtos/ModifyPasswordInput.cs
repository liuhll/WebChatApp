using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeuci.WeChatApp.WechatAccount.Dtos
{
    public class ModifyPasswordInput
    {
        public string OpenId { get; set; }

        public string AccountName { get; set; }

        public string OldPassword { get; set; }

        public string NewPassworld { get; set; }

        
    }
}
