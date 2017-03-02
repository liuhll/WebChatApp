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
            var nowTime = DateTime.Now;
            var orderId = nowTime.ToString("yyyyMMdd") + (nowTime.Hour * 3600 + nowTime.Minute * 60 + nowTime.Second).ToString("D5") + GenerateRandomNumber(7);
            return string.Format("{0}{1}", WxPayConfig.MCHID, orderId);
        }

       public static string GenerateNewId()
       {
            var nowTime = DateTime.Now;
            var orderId = nowTime.ToString("yyyyMMdd") + (nowTime.Hour * 3600 + nowTime.Minute * 60 + nowTime.Second).ToString("D5") + GenerateRandomNumber(7);
           return orderId;
       }

        private static char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public static string GenerateRandomNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(10);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(10)]);
            }
            return newRandom.ToString();
        }

    }
}
