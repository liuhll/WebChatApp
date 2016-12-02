using Abp.AutoMapper;
using Jeuci.WeChatApp.Common.Enums;

namespace Jeuci.WeChatApp.WechatAccount.Dtos
{
    [AutoMap(typeof(Wechat.Models.Account.WechatAccount))]
    public class WechatAccountOutput
    {
        public string Subscribe { get; set; }

        public string OpenId { get; set; }

        public string NickName { get; set; }

        public string HeadImgUrl { get; set; }


 
    }
}
