using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Json;
using Abp.Logging;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Wechat.Authentication;
using Jueci.WeChatApp.RestfulRequestTool.Common.Enums;
using Jueci.WeChatApp.RestfulRequestTool.RequestAbstractions;
using Jueci.WeChatApp.RestfulRequestTool.RequestAbstractions.Impl;
using Newtonsoft.Json;
using Nito.AsyncEx;
using Senparc.Weixin;
using Senparc.Weixin.MP.AppStore;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities.Menu;

namespace Jeuci.WeChatApp.Wechat.Menu
{
    public class WechatMenuManager : IWechatMenuManager
    {
        private readonly IWechatAuthentManager _wechatAuthentManager;

        private readonly IWechatMenuProvider _wechatMenuProvider;
        private readonly IRequestFactory _requestFactory;
        public WechatMenuManager(IWechatAuthentManager wechatAuthentManager,
            IWechatMenuProvider wechatMenuProvider)
        {
            _wechatAuthentManager = wechatAuthentManager;
            _wechatMenuProvider = wechatMenuProvider;
            //_requestFactory = new RequestFactory(ConfigHelper.GetValuesByKey("WechatUri"));
        }

        public bool CreateWechatMenu(out string msg)
        {
            var accessToken = _wechatAuthentManager.GetAccessToken();
            if (!string.IsNullOrEmpty(accessToken))
            {
                var wechatMenu = _wechatMenuProvider.GetWechatMenu();

                try
                {
                    var result = CommonApi.CreateMenu(accessToken, wechatMenu);
                    msg = result.errmsg;
                    return result.errcode == ReturnCode.请求成功;
                }
                catch (Exception e)
                {
                    LogHelper.Logger.Error(e.Message,e);
                    msg = e.Message;
                }
                return false;
            }
            msg = "获取AcessToken失败";
            return false;
        }
    }
}