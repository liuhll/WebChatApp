using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Logging;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Services.Email;
using Jeuci.WeChatApp.Wechat.Models.Account;
using Jeuci.WeChatApp.Wechat.Models.BindEmail;
using Jeuci.WeChatApp.Wechat.Policy;

namespace Jeuci.WeChatApp.Wechat.Accounts.Impl
{
    public class BindEmailProcessor : IBindEmailProcessor
    {
        private readonly IDirectEmailService _directEmailService;

        private readonly IRepository<UserInfo> _useRepository;

        public BindEmailProcessor(IDirectEmailService directEmailService,
            IRepository<UserInfo> useRepository)
        {
            _directEmailService = directEmailService;
            _useRepository = useRepository;
        }

        public Task<bool> SendValidByEmail(string openId, string emailAddress)
        {
            var bindEmailPolicy = new EmailPolicy();
            var validCode = bindEmailPolicy.GetBindEmailValidCode(EmailValidCodeType.BindEmail);
            CacheHelper.SetCache(openId,validCode);
            var emailBody = bindEmailPolicy.GetEmailBody(validCode);
            return _directEmailService.SendValidCodeByEmail(emailAddress, emailBody,"彩盟网邮箱绑定验证码");
        }

        public bool BindUserEmail(BindEmailModel model, out string msgOrUrl)
        {
            var validCode = CacheHelper.GetCache<EmailValidCode>(model.OpenId);
            if (validCode == null)
            {
                LogHelper.Logger.Info("缓存过期，没有获取到电子邮件验证码");
                throw new Exception("验证码超时，请重新获取电子邮件验证码");
            }
            var bindEmailPolicy = new EmailPolicy();
            if (!bindEmailPolicy.ValidEmailCode(validCode,model.ValidCode,out msgOrUrl))
            {               
                return false;
            }
            var jeuciAccount = new JeuciAccount(model.OpenId,model.AccountName,model.Password,AccountOperateType.BindEmail);
            jeuciAccount.SynchronUserInfo(_useRepository);           
            var jueciAccountPolicy = new JeuciAccountPolicy(jeuciAccount);      
            if (!jueciAccountPolicy.ValidAccountLegality(out msgOrUrl))
            {
                return false;
            }

            jeuciAccount.UserInfo.Email = model.Email;
            try
            {
                _useRepository.Update(jeuciAccount.UserInfo);
                msgOrUrl = string.Format("/wechat/account/#/wechatforjeuci?isNeedCallBack=False&openId={0}",model.OpenId);
                return true;
            }
            catch (Exception e)
            {
                msgOrUrl = e.Message;
                return false;
            }
        }
    }
}
