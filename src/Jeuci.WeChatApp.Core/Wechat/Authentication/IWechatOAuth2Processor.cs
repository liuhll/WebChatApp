using Abp.Dependency;
using Jeuci.WeChatApp.Common;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

namespace Jeuci.WeChatApp.Wechat.Authentication
{
    public interface IWechatOAuth2Processor : ITransientDependency
    {
        ResultMessage<OAuthUserInfo> GetWechatUserInfo(string code,string state);

        string GetWechatAuthorizeUrl(string redirectUrl, string state, OAuthScope oAuthScope);
    }
}