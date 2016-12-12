using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Logging;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;

namespace Jeuci.WeChatApp.Wechat.Models.BindEmail
{
    public class EmailValidCode
    {
        private readonly DateTime _generateTime;
        private readonly string _validCode;
        private readonly int _validDuration;
        private readonly EmailValidCodeType _emailValidCodeType;

        public EmailValidCode(string validCode, EmailValidCodeType emailValidCodeType)
        {
            _validCode = validCode;
            _emailValidCodeType = emailValidCodeType;
            _generateTime = DateTime.Now;
            _validDuration = ConfigHelper.GetIntValues("emailValidDuration");
        }

        public DateTime GenerateTime
        {
            get { return _generateTime; }
        }

        public EmailValidCodeType EmailValidCodeType
        {
            get { return _emailValidCodeType; }
        }

        public string ValidCode
        {
            get
            {
                if (!IsEffectiveValidCode)
                {
                    LogHelper.Logger.Error("获取验证码失败，该验证码超时");
                    throw new Exception("邮箱验证码失效，请重新获取");
                }
                return _validCode;
            }
        }

        public bool IsEffectiveValidCode
        {
            get { return DateTime.Now < _generateTime.AddMinutes(_validDuration) ; }
        }

        public int ValidDuration
        {
            get { return _validDuration; }
        }

    }
}
