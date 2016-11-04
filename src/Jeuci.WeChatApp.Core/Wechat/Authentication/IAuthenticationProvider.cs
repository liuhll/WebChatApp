using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Jeuci.WeChatApp.Wechat.Models;

namespace Jeuci.WeChatApp.Wechat.Authentication
{
    public interface IAuthenticationProvider : ITransientDependency
    {
        string GetWechatToken();
        string GetWechatSignStrByParams(string token, string timestamp, string nonce);
    }
}
