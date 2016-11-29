using System.Web.Http;
using Abp.Application.Services;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Wechat.Models.Account;
using Jeuci.WeChatApp.WeChatAuth.Dtos;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Entities;

namespace Jeuci.WeChatApp.WeChatAuth
{
    public interface IWechatAuthAppService : IApplicationService
    {
        bool CheckSignature(WechatSignInput input);

        [HttpGet]
        ResultMessage<Wechat.Models.Account.WechatAccount> GetWechatUserInfo(string code, string state);

        [HttpGet]
        string GetWechatAuthorizeUrl(string redirectUrl, string state, int oAuthScope);

        [HttpGet]
        ResultMessage<string> GetWechatUserOpenId(string code, string state);
    }
}