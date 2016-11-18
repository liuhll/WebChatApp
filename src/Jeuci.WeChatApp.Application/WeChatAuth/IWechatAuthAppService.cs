using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Dependency;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Wechat.Models;
using Jeuci.WeChatApp.WeChatAuth.Dtos;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

namespace Jeuci.WeChatApp.WeChatAuth
{
    public interface IWechatAuthAppService : IApplicationService
    {
        bool CheckSignature(WechatSignInput input);

        [HttpGet]
        ResultMessage<OAuthUserInfo> GetWechatUserInfo(string code, string state);

        [HttpGet]
        string GetWechatAuthorizeUrl(string redirectUrl, string state, int oAuthScope);

    }
}