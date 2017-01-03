using Abp.Domain.Entities;
using Jeuci.WeChatApp.Common.Enums;

namespace Jeuci.WeChatApp.Wechat.Models.Account
{
    public class UserInfo : Entity
    {
        public string UserName { get; set; }

        public string Mobile { get; set; }

        public string Password { get; set; }

        public string PayPassword { get; set; }

        public string SafeEmail { get; set; }

        public string WeChat { get; set; }

        public string NickName { get; set; }

        public Sex? Sex { get; set; }

        public decimal Fund { get; set; }

    }
}