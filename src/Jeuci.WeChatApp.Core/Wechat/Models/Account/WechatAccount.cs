using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeuci.WeChatApp.Common.Enums;

namespace Jeuci.WeChatApp.Wechat.Models.Account
{
    public class WechatAccount
    {
        public string Subscribe { get; set; }

        public string OpenId { get; set; }

        public string NickName { get; set; }

        public Sex Sex { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Province { get; set; }

        public string Language { get; set; }

        public string HeadImgUrl { get; set; }

        public int Subscribe_Time { get; set; }

        public string Unionid { get; set; }

        public string Remark { get; set; }

        public string GroupId { get; set; }

    }
}
