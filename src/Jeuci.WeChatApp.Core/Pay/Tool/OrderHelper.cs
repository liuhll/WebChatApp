using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeuci.WeChatApp.Pay.Tool
{
   public class OrderHelper
    {
        public static string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}{2}", WxPayConfig.MCHID, DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999999));
        }

       public static string GenerateNewId()
       {
            var ran = new Random();
            return string.Format("{0}{1}",DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999999));
        }
    }
}
