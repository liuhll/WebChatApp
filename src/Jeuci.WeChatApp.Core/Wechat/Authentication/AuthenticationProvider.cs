using System;
using System.Collections;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Wechat.Models;

namespace Jeuci.WeChatApp.Wechat.Authentication
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        public virtual string GetWechatToken()
        {

            return ConfigHelper.GetValuesByKey("WechatToken");
        }

        public virtual string GetWechatSignStrByParams(string token, string timestamp, string nonce)
        {
            var ar = new[] {token,timestamp,nonce};
            Array.Sort(ar);
            return string.Join(null,ar);
        }
    }
}