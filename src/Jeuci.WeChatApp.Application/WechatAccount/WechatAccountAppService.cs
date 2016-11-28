using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Wechat.Authentication;
using Jeuci.WeChatApp.Wechat.Models.Account;
using Senparc.Weixin.MP.Entities;

namespace Jeuci.WeChatApp.WechatAccount
{
    public class WechatAccountAppService : IWechatAccountAppService
    {
        private readonly IWechatOAuth2Processor _wechatOAuth2Processor;

        public WechatAccountAppService(IWechatOAuth2Processor wechatOAuth2Processor)
        {
            _wechatOAuth2Processor = wechatOAuth2Processor;
        }

        public ResultMessage<JeuciAccount> GetWechatUserInfo(string openId)
        {
            return _wechatOAuth2Processor.GetWechatUserInfo(openId);
        }
    }
}
