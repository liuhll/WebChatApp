using Abp.AutoMapper;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.WechatAccount.Dtos
{
    [AutoMap(typeof(UserInfo))]
    public class UserInfoOutput
    {

        public string UserName { get; set; }

        public string Mobile { get; set; }

        public string SafeEmail { get; set; }

        public string WeChat { get; set; }

        public string NickName { get; set; }

    }
}
