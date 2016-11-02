using System;
using Abp;

namespace Jeuci.WeChatApp.Common.Exceptions
{
    public class ReadEmailTempletException : AbpException
    {
        public ReadEmailTempletException(string msg) : base(msg)
        {
        }

        public ReadEmailTempletException(string message, Exception innerException)
         : base(message, innerException)
        {
        }
    }
}