using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using Jeuci.WeChatApp.Common;
using Senparc.Weixin.MP.Entities;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.WechatAccount
{
    public interface IWechatAccountAppService : IApplicationService
    {
        [HttpGet]
        ResultMessage<JeuciAccount> GetWechatUserInfo(string openId);
    }
}
