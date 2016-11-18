using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Jeuci.WeChatApp.Wechat.Models;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.Wechat.Authentication
{
    public interface IWechatAuthentManager : ITransientDependency
    {
        bool CheckSignature(WechatSign wechatSign);

        string GetAccessToken();


    }
}