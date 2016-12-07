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

        public bool ValidEmailCode(EmailValidCode validCode,string validCodeStr, out string msgOrUrl)
        {
            if (!validCode.IsEffectiveValidCode)
            {
                msgOrUrl = "验证码失效，请重新获取验证码";
                return false;
            }
            if (!validCodeStr.Equals(validCode.ValidCode))
            {
                msgOrUrl = "您输入的验证码不正确，请重新输入!";
                return false;
            }
            msgOrUrl = "Ok";
            return true;

        }
    }
}
