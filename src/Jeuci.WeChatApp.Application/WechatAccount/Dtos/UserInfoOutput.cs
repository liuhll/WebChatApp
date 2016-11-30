﻿using Abp.AutoMapper;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.WechatAccount.Dtos
{
    [AutoMap(typeof(UserInfo))]
    public class UserInfoOutput
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string WeChat { get; set; }

        public string NickName { get; set; }

        public Sex Sex { get; set; }

        public decimal Fund { get; set; }
    }
}