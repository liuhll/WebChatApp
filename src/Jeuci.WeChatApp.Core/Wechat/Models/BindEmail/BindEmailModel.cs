using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeuci.WeChatApp.Wechat.Models.BindEmail
{
    public class BindEmailModel
    {
        public string OpenId { get; set; }

        public string AccountName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string ValidCode { get; set; }
    }
}
