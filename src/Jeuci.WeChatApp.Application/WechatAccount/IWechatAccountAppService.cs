using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Wechat.Models.Account;
using Jeuci.WeChatApp.WechatAccount.Dtos;

namespace Jeuci.WeChatApp.WechatAccount
{
    public interface IWechatAccountAppService : IApplicationService
    {
        [HttpGet]
        ResultMessage<JeuciAccountOutput> GetWechatUserInfo(string openId);

        [HttpPost]
        ResultMessage<string> BindWechatAccount(BindAccountInput input);

        [HttpPost]
        ResultMessage<string> UnbindWechatAccount(UnBindAccountInput input);

        [HttpPut]
        ResultMessage<string> Password(ModifyPasswordInput input);

        [HttpGet]
        Task<ResultMessage<string>> RetrievePwdValidCode(string openId, string email);

        [HttpPut]
        ResultMessage<string> RetrievePwd(RetrievePwdInput input);



    }
}
