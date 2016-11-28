using Abp.Dependency;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Wechat.Models.Account;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Entities;

namespace Jeuci.WeChatApp.Wechat.Authentication
{
    public interface IWechatOAuth2Processor : ITransientDependency
    {
        ResultMessage<JeuciAccount> GetWechatUserInfo(string code,string state);

        ResultMessage<JeuciAccount> GetWechatUserInfo(string openId);

        string GetWechatAuthorizeUrl(string redirectUrl, string state, OAuthScope oAuthScope);

        ResultMessage<string> GetWechatUserOpenId(string code, string state);
    }
}