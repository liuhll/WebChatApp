using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Abp;
using Abp.AutoMapper;
using Abp.Logging;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Wechat.Authentication;
using Jeuci.WeChatApp.Wechat.Models;
using Jeuci.WeChatApp.Wechat.Models.Account;
using Jeuci.WeChatApp.WeChatAuth.Dtos;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

namespace Jeuci.WeChatApp.WeChatAuth
{
    public class WechatAuthAppService : IWechatAuthAppService
    {
        private readonly IWechatAuthentManager _wechatAuthentManager;

        private readonly IWechatOAuth2Processor _wechatOAuth2Processor; 

        public WechatAuthAppService(IWechatAuthentManager wechatAuthentManager,
            IWechatOAuth2Processor wechatOAuth2Processor)
        {
            _wechatAuthentManager = wechatAuthentManager;
            _wechatOAuth2Processor = wechatOAuth2Processor;
        }

        public bool CheckSignature(WechatSignInput input)
        {
            return _wechatAuthentManager.CheckSignature(input.MapTo<WechatSign>());
        }

        public ResultMessage<OAuthUserInfo> GetWechatUserInfo(string code, string state)
        {
            return _wechatOAuth2Processor.GetWechatUserInfo(code,state);
        }

 
        public string GetWechatAuthorizeUrl(string redirectUrl, string state, int oAuthScope)
        {
            OAuthScope oAuthScopeEnum;
            try
            {
                oAuthScopeEnum = (OAuthScope) Enum.ToObject(typeof(OAuthScope), oAuthScope);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(@"转化微信授权类型失败，类型取值仅能为0||1",ex);
                throw new AbpException(@"转化微信授权类型失败，类型取值仅能为0||1",ex);
            }

            return _wechatOAuth2Processor.GetWechatAuthorizeUrl(redirectUrl,state, oAuthScopeEnum);
        }
    }
}