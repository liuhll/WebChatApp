using System;
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
using Senparc.Weixin.MP.Entities;

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

        public ResultMessage<JeuciAccount> GetWechatUserInfo(string openId)
        {
            return _wechatOAuth2Processor.GetWechatUserInfo(openId);
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
