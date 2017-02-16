using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeuci.WeChatApp.Pay.Lib
{
    class WxPayException : Exception
    {
        public WxPayException(string msg) : base(msg)
        {

        }
    }
}
