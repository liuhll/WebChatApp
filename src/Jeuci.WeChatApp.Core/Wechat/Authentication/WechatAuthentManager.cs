﻿using System.Security.Authentication;
using Abp.Logging;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Wechat.Models;
using Newtonsoft.Json;
using WeixinSdk=Senparc.Weixin.MP;

namespace Jeuci.WeChatApp.Wechat.Authentication
{
    public class WechatAuthentManager : IWechatAuthentManager
    {
        private readonly IAuthenticationProvider _permissionProvider;

        public WechatAuthentManager(IAuthenticationProvider permissionProvider)
        {
            _permissionProvider = permissionProvider;
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

        //protected virtual bool CheckSignature(WechatSign wechatSign, string token)
        //{
        //    string tempStr = _permissionProvider.GetWechatSignStrByParams(token,wechatSign.Timestamp.ToString(),wechatSign.Nonce.ToString());
        //    LogHelper.Logger.Info("得到的签名串为:"+tempStr);
        //    tempStr = EncryptionHelper.EncryptSHA(tempStr);
        //    LogHelper.Logger.Info("加密后的串为:"+tempStr);
        //    return tempStr.Equals(wechatSign.Signature);

        //}


    }
}