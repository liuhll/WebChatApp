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
using Jeuci.WeChatApp.WechatAccount.Dtos;
using Jeuci.WeChatApp.WeChatAuth.Dtos;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Entities;

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

        public ResultMessage<WechatAccountOutput> GetWechatUserInfo(string code, string state)
        {
            string msg = string.Empty;
            try
            {
                var wechatAccount = _wechatOAuth2Processor.GetWechatUserInfo(code,state,out msg);
                if (wechatAccount == null)
                {
                    return new ResultMessage<WechatAccountOutput>(ResultCode.Fail, msg);
                }
                return new ResultMessage<WechatAccountOutput>(wechatAccount.MapTo<WechatAccountOutput>(), msg);
            }
            catch (Exception e)
            {
                return new ResultMessage<WechatAccountOutput>(ResultCode.Fail, e.Message);
            }
            
        }

       
        public string GetWechatAuthorizeUrl(string redirectUrl, string state, int oAuthScope)
        {
            OAuthScope oAuthScopeEnum;
            if (oAuthScope == 0 || oAuthScope == 1)
            {
                oAuthScopeEnum = (OAuthScope)Enum.ToObject(typeof(OAuthScope), oAuthScope);
            }
            else
            {
                LogHelper.Logger.Error(@"转化微信授权类型失败，类型取值仅能为0||1");
                throw new Exception("转化微信授权类型失败，类型取值仅能为0 || 1");
            }
            return _wechatOAuth2Processor.GetWechatAuthorizeUrl(redirectUrl,state, oAuthScopeEnum);
        }

        public ResultMessage<string> GetWechatUserOpenId(string code, string state)
        {
            try
            {
                var openId = _wechatOAuth2Processor.GetWechatUserOpenId(code, state);
           
                return !string.IsNullOrEmpty(openId) ? new ResultMessage<string>(openId) : new ResultMessage<string>(ResultCode.Fail,"获取OpenId失败，请确保您已经关注了我们！");
            }
            catch (Exception e)
            {
                return new ResultMessage<string>(ResultCode.ServiceError, e.Message);
            }
        }
    }
}