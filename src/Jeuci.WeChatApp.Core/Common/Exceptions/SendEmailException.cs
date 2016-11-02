using System;
using Abp;

namespace Jeuci.WeChatApp.Common.Exceptions
{
    public class SendEmailException : AbpException
    {
        public SendEmailException(string msg) : base(msg)
        {
        }

        public SendEmailException(string message, Exception innerException)
         : base(message, innerException)
        {
        }                                                            
    }
}