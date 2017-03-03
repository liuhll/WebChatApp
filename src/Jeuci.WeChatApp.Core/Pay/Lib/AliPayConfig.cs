using System;
using System.IO;
using Jeuci.WeChatApp.Common.Tools;
using Senparc.Weixin.HttpUtility;

namespace Jeuci.WeChatApp.Pay.Lib
{
    public class AliPayConfig
    {
        public static string APPID
        {
            get
            {
                return ConfigHelper.GetValuesByKey("alipayAppId");
            }
        }

        public static string APP_PRIVATE_KEY
        {
            get
            {
                var appPriviteKey = ConfigHelper.GetValuesByKey("appPriviteKey");

                return appPriviteKey.Replace("\r", "").Replace("\n", "").Replace(" ", "");

            }
        }

        public static string ALIPAY_PUBLIC_KEY
        {
            get
            {
                var appPriviteKey = ConfigHelper.GetValuesByKey("alipayPublicKey");

                return appPriviteKey.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            }
        }

        public const string CHARSET = "UTF-8";

        public static string SERVER_URL
        {
            get { return ConfigHelper.GetValuesByKey("alipayServerUrl"); }
        }

        public static string RETURN_URL
        {
            get { return ConfigHelper.GetValuesByKey("alipayReturnUrl"); }
        }

        public static string NOTIFY_URL
        {
            get { return ConfigHelper.GetValuesByKey("WechatServiceAddress") + "/api/Alipay/WapPay"; }
        }

        public static string PID
        {
            get { return ConfigHelper.GetValuesByKey("alipayPid"); }
        }

        public const string SIGN_TYPE = "RSA2";

        public const string FORMAT = "json";

    }
}