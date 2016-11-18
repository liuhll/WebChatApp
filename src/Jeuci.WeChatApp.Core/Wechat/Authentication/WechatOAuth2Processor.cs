using System;
using System.Web.Caching;
using System.Web.UI.WebControls;
using Abp.Logging;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;
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

        public WechatOAuth2Processor()
        {
            _appid = ConfigHelper.GetValuesByKey("WechatAppid");
            _appsecret = ConfigHelper.GetValuesByKey("WechatAppSecret");
        }

        public ResultMessage<OAuthUserInfo> GetWechatUserInfo(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                LogHelper.Logger.Error("被拒绝了的授权,请确保用户已经关注了公众号");
                return new ResultMessage<OAuthUserInfo>(ResultCode.Fail, "被拒绝了的授权,请确保用户已经关注了公众号");
            }

            if (state != CacheHelper.GetCache<string>("State"))
            {
                LogHelper.Logger.Error("验证失败！请从正规途径进入！");
                return new ResultMessage<OAuthUserInfo>(ResultCode.Fail, "验证失败！请从正规途径进入！");

            }

            OAuthAccessTokenResult result = null;
            try
            {
                result = OAuthApi.GetAccessToken(_appid, _appsecret, code);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("错误：" + ex.Message, ex);
                return new ResultMessage<OAuthUserInfo>(ResultCode.Fail, "错误：" + ex.Message);

            }

            if (result.errcode != ReturnCode.请求成功)
            {
                LogHelper.Logger.Error("错误：" + result.errmsg);
                return new ResultMessage<OAuthUserInfo>(ResultCode.Fail, "错误：" + result.errmsg);
            }

            CacheHelper.SetCache("OAuthAccessTokenStartTime", DateTime.Now);
            CacheHelper.SetCache("OAuthAccessToken", result);

            try
            {
                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                LogHelper.Logger.Info("成功：获取微信用户个人信息成功" + result.errmsg);
                return new ResultMessage<OAuthUserInfo>(userInfo, "OK");
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("错误：" + e.Message);
                return new ResultMessage<OAuthUserInfo>(ResultCode.Fail, "错误:" + e.Message);
            }
        }

        public string GetWechatAuthorizeUrl(string redirectUrl, string state, OAuthScope oAuthScope)
        {
            CacheHelper.SetCache("State",state);           
            string wxAuthorizeUrl = OAuthApi.GetAuthorizeUrl(_appid, redirectUrl, state, oAuthScope);
            LogHelper.Logger.Info(string.Format("要回调的地址为：{0}获取到微信授权服务器的地址为：{1}",redirectUrl,wxAuthorizeUrl));
            return wxAuthorizeUrl;
        }
    }
}