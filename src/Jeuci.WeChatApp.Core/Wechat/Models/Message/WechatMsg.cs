using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace Jeuci.WeChatApp.Wechat.Models.Message
{
    public class WechatMsg : Entity
    {
        public string KeyWord { get; set; }

        public int? Sid { get; set; }

        public string ResponseMsg { get; set; }

        public DateTime CreateTime { get; set; }


    }
}
