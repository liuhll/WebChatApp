using System;
using Abp;

namespace Jeuci.WeChatApp.Common.Exceptions
{
    public class OrderException : AbpException
    {
        public OrderException(string msg) : base(msg)
        {
        }

       
    }
}