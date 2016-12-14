using System;
using System.Web.Caching;
using System.Web.UI.WebControls;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Logging;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Wechat.Models.Account;
using Newtonsoft.Json;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

namespace Jeuci.WeChatApp.Wechat.Authentication
{
    public class WechatOAuth2Processor : IWechatOAuth2Processor
    {
        private readonly string _appid;

        private readonly string _appsecret;

        private readonly IWechatAuthentManager _wechatAuthentManager;
        private readonly IRepository<UserInfo> _userRepository;

        public WechatOAuth2Processor(IWechatAuthentManager wechatAuthentManager,
            IRepository<UserInfo> userRepository)
        {
            _wechatAuthentManager = wechatAuthentManager;
            _userRepository = userRepository;
            _appid = ConfigHelper.GetValuesByKey("WechatAppid");
            _appsecret = ConfigHelper.GetValuesByKey("WechatAppSecret");
        }

        public WechatAccount GetWechatUserInfo(string code, string state,out string resultMsg)
        {
            string resultMessage;
            OAuthAccessTokenResult accessTokenResult;
            var result = GetWechatOAuthAccessTokenResult(code, state, out resultMessage,out accessTokenResult);

            resultMsg = resultMessage;
            if (!result) return null;
            var userInfo = OAuthApi.GetUserInfo(accessTokenResult.access_token, accessTokenResult.openid);
            LogHelper.Logger.Info("获取用户信息成功：获取微信用户个人信息成功" + accessTokenResult.errmsg);
            resultMsg = "OK;" + accessTokenResult.errmsg;

            return userInfo.MapTo<WechatAccount>();
        }

        public JeuciAccount GetWechatUserInfo(string openId)
        {
            var jeuciAccount = new JeuciAccount(openId,AccountOperateType.ObtainAccount);
            jeuciAccount.SynchronWechatUserInfo(_wechatAuthentManager);
            jeuciAccount.SynchronUserInfo(_userRepository);
            return jeuciAccount;
 
        }

        public string GetWechatUserOpenId(string code, string state)
        {
            string resultMessage;
            OAuthAccessTokenResult accessTokenResult;
            var result = GetWechatOAuthAccessTokenResult(code, state, out resultMessage, out accessTokenResult);
            if (result)
            {
                return accessTokenResult.openid;
            }
            return string.Empty;
        }
       
        public string GetWechatAuthorizeUrl(string redirectUrl, string state, OAuthScope oAuthScope)
        {
            CacheHelper.SetCache("State",state);           
            string wxAuthorizeUrl = OAuthApi.GetAuthorizeUrl(_appid, redirectUrl, state, oAuthScope);
            LogHelper.Logger.Info(string.Format("要回调的地址为：{0}获取到微信授权服务器的地址为：{1}",redirectUrl,wxAuthorizeUrl));
            return wxAuthorizeUrl;
        }

        private bool GetWechatOAuthAccessTokenResult(string code, string state, out string resultMessage, out OAuthAccessTokenResult accessTokenResult)
        {
            if (string.IsNullOrEmpty(code))
            {

                resultMessage = "被拒绝了的授权,请确保用户已经关注了公众号";
                LogHelper.Logger.Error(resultMessage);
                accessTokenResult = null;
                return false;
            }

            if (state != CacheHelper.GetCache<string>("State"))
            {
                resultMessage = "验证失败！请从正规途径进入！";
                LogHelper.Logger.Error(resultMessage);
                accessTokenResult = null;
                return false;
            }

            OAuthAccessTokenResult result = null;
            try
            {
                LogHelper.Logger.Info(string.Format("{0},{1},{2}", _appid, _appsecret, code));
                result = OAuthApi.GetAccessToken(_appid, _appsecret, code);
                LogHelper.Logger.Info("获取到的accessToken为：" + JsonConvert.SerializeObject(result));
                accessTokenResult = result;
            }
            catch (Exception ex)
            {
                resultMessage = "获取accessToken失败：" + ex.Message;
                LogHelper.Logger.Error(resultMessage, ex);
                accessTokenResult = null;
                return false;
            }

            if (result.errcode != ReturnCode.请求成功)
            {
                LogHelper.Logger.Error("获取accessToken失败：" + result.errmsg);
                {
                    resultMessage = "获取accessToken失败：" + result.errmsg;
                    LogHelper.Logger.Error(resultMessage);
                    return false;

                }
            }

            CacheHelper.SetCache("OAuthAccessTokenStartTime", DateTime.Now);
            CacheHelper.SetCache("OAuthAccessToken", result);
            resultMessage = "获取accessToken成功：" + result.errmsg;
            LogHelper.Logger.Error(resultMessage);
            return true;
        }

    }
}