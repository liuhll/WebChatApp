using System.Web.Http;
using Abp.Application.Services;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.WechatAccount.Dtos;
using Jeuci.WeChatApp.WeChatAuth.Dtos;

namespace Jeuci.WeChatApp.WeChatAuth
{
    public interface IWechatAuthAppService : IApplicationService
    {
        bool CheckSignature(WechatSignInput input);

        [HttpGet]
        ResultMessage<WechatAccountOutput> GetWechatUserInfo(string code, string state);

        [HttpGet]
        string GetWechatAuthorizeUrl(string redirectUrl, string state, int oAuthScope);

        [HttpGet]
        ResultMessage<string> GetWechatUserOpenId(string code, string state);
    }
}