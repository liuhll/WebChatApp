using System.Security.Authentication;
using Abp.Logging;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Wechat.Models;
using Newtonsoft.Json;
using Senparc.Weixin.MP.Containers;
using WeixinSdk=Senparc.Weixin.MP;

namespace Jeuci.WeChatApp.Wechat.Authentication
{
    public class WechatAuthentManager : IWechatAuthentManager
    {
        private readonly IAuthenticationProvider _permissionProvider;

        private readonly string _appid;

        private readonly string _appsecret;

        public WechatAuthentManager(IAuthenticationProvider permissionProvider)
        {
            _permissionProvider = permissionProvider;

            _appid = ConfigHelper.GetValuesByKey("WechatAppid");
            _appsecret = ConfigHelper.GetValuesByKey("WechatAppSecret");


        }

        public bool CheckSignature(WechatSign wechatSign)
        {
            string apiName = "微信平台接入";
            LogHelper.Logger.Info(string.Format(MessageTips.StartCallApiInfo, apiName, JsonConvert.SerializeObject(wechatSign)));
            var token = _permissionProvider.GetWechatToken();
            if (string.IsNullOrEmpty(token))
            {
                LogHelper.Logger.Error(MessageTips.NoToken);
                throw new AuthenticationException(MessageTips.NoToken);
            }
            
            if (WeixinSdk.CheckSignature.Check(wechatSign.Signature, wechatSign.Timestamp.ToString(), wechatSign.Nonce.ToString(), token))
            {
                LogHelper.Logger.Info(string.Format(MessageTips.EndCallApiInfoBySuccess, apiName));
                return true;
            }
            LogHelper.Logger.Error(string.Format(MessageTips.EndCallApiInfoByFailure, apiName,"签名认证失败"));
            return false;
        }

        public string GetAccessToken()
        {
            var accessToken = AccessTokenContainer.TryGetAccessToken(_appid, _appsecret);
            return accessToken;
        }
    }
}