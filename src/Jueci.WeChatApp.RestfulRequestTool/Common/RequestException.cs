using System;
using Abp;

namespace Jueci.WeChatApp.RestfulRequestTool.Common
{
    public class RequestException : AbpException
    {
        public RequestException(string message)
        : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="T:Abp.AbpException" /> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public RequestException(string message, Exception innerException)
        : base(message, innerException)
        {
        }
    }
}