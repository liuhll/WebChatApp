using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeuci.WeChatApp.Services.Email;

namespace Jeuci.WeChatApp.Wechat.Accounts.Impl
{
    public class BindEmailProcessor : IBindEmailProcessor
    {
        private readonly IDirectEmailService _directEmailService;

        public BindEmailProcessor(IDirectEmailService directEmailService)
        {
            _directEmailService = directEmailService;
        }

        public Task<bool> SendValidByEmial(string openId, string emailAddress)
        {
            return _directEmailService.SendValidCodeByEmial(emailAddress,"测试邮件发送");
        }
    }
}
