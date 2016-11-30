﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Logging;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Wechat.Accounts;
using Jeuci.WeChatApp.Wechat.Authentication;
using Jeuci.WeChatApp.Wechat.Models.Account;
using Jeuci.WeChatApp.WechatAccount.Dtos;
using Newtonsoft.Json;

namespace Jeuci.WeChatApp.WechatAccount
{
    public class WechatAccountAppService : IWechatAccountAppService
    {
        private readonly IWechatOAuth2Processor _wechatOAuth2Processor;
        private readonly IBindAccountProcessor _bindAccountProcessor;

        public WechatAccountAppService(IWechatOAuth2Processor wechatOAuth2Processor,
            IBindAccountProcessor bindAccountProcessor)
        {
            _wechatOAuth2Processor = wechatOAuth2Processor;
            _bindAccountProcessor = bindAccountProcessor;
        }

        public ResultMessage<JeuciAccountOutput> GetWechatUserInfo(string openId)
        {
            try
            {
                var jeuciAccount = _wechatOAuth2Processor.GetWechatUserInfo(openId);
                LogHelper.Logger.Info(JsonConvert.SerializeObject(jeuciAccount.MapTo<JeuciAccountOutput>()));
                return new ResultMessage<JeuciAccountOutput>(jeuciAccount.MapTo<JeuciAccountOutput>());
            }
            catch (Exception e)
            {
                return new ResultMessage<JeuciAccountOutput>(ResultCode.Fail,e.Message);
            }
        }

        public ResultMessage<string> BindWechatAccount(BindAccountInput input)
        {
            try
            {
                return _bindAccountProcessor.BindWechatAccount(new JeuciAccount(input.OpenId,input.Account,input.Password));
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error(e.Message);
                return new ResultMessage<string>(ResultCode.Fail,e.Message);
            }
        }
    }
}
