using Abp.AutoMapper;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.WechatAccount.Dtos
{
    [AutoMap(typeof(JeuciAccount))]
    public class JeuciAccountOutput
    {
        public bool IsBindWechat{ get; set; }

        public bool IsBindEmail { get; set; }

        public string OpenId { get; set; }

        public string BindWechatAddress { get; set; }

        public string BindEmailAddress { get; set; }

        public UserInfoOutput UserInfo { get; set; }

        public WechatAccountOutput UserWechatInfo { get;set; }
    }
}