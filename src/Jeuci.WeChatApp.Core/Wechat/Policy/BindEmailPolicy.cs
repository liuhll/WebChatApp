using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Wechat.Models.BindEmail;

namespace Jeuci.WeChatApp.Wechat.Policy
{
    public class BindEmailPolicy
    {
        public EmailValidCode GetBindEmailValidCode()
        {
            var validCode = RandomHelper.GenerateString(6);
            return new EmailValidCode(validCode);
        }

        public string GetEmailBody(EmailValidCode validCode)
        {
            return "【彩盟网】  您要绑定的邮箱的验证码为:" + validCode.ValidCode + ",有效期为:" + validCode.ValidDuration + "分钟,请在效期内完成绑定操作！";
        }
    }
}
