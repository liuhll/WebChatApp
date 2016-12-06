using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Services.Email;
using Jeuci.WeChatApp.Wechat.Policy;

namespace Jeuci.WeChatApp.Wechat.Accounts.Impl
{
    public class BindEmailProcessor : IBindEmailProcessor
    {
        private readonly IDirectEmailService _directEmailService;

        public BindEmailProcessor(IDirectEmailService directEmailService)
        {
            _directEmailService = directEmailService;
        }

        public Task<bool> SendValidByEmail(string openId, string emailAddress)
        {
            var bindEmailPolicy = new BindEmailPolicy();
            var validCode = bindEmailPolicy.GetBindEmailValidCode();
            CacheHelper.SetCache(openId,validCode);
            var emailBody = bindEmailPolicy.GetEmailBody(validCode);
            return _directEmailService.SendValidCodeByEmail(emailAddress, emailBody);
        }
    }
}
