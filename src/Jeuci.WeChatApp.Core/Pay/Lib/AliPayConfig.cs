using System;
using System.IO;
using Jeuci.WeChatApp.Common.Tools;

namespace Jeuci.WeChatApp.Pay.Lib
{
    public class AliPayConfig
    {
        public const string APPID = "2016072800110782";

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

        public const string SERVER_URL = "https://openapi.alipaydev.com/gateway.do";

        public static string RETURN_URL
        {
            get { return "http://www.camew.com/"; }
        }

        public static string NOTIFY_URL
        {
            get { return ConfigHelper.GetValuesByKey("WechatServiceAddress") + "/api/Alipay/WapPay"; }
        }

        public const string PID = "2088102168867788";

        public const string SIGN_TYPE = "RSA2";

        public const string FORMAT = "json";

    }
}