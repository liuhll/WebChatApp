using System;

namespace Jeuci.WeChatApp.Pay.Tool
{
    public class NonceHelper
    {
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}