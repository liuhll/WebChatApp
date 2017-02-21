using System;
using Jeuci.WeChatApp.Pay.Tool;

namespace Jeuci.WeChatApp.Pay.Models
{
    public class ServiceOrder
    {
        public string appid {
            get { return WxPayConfig.APPID; }
        }

        public string mch_id
        {
            get { return WxPayConfig.MCHID; }
        }

        public string device_info
        {
            get { return "WEB"; }
        }

        public string nonce_str {
            get { return NonceHelper.GenerateNonceStr(); }
        }

        public string sign_type
        {
            get
            {
                return "MD5";
            }
        }

        public string body { get; set; }

        public string detail { get; set; }

        public string attach { get; set; }

        public string out_trade_no {
            get { return OrderHelper.GenerateOutTradeNo(); }
        }

        public int total_fee { get; set; }

        public string spbill_create_ip {
            get { return IpAddressHelper.GetLocalIpAddress(); }
        }

        public string time_start
        {
            get { return DateTime.Now.ToString("yyyyMMddHHmmss"); }
        }

        //public string time_expire
        //{
        //    get { return WxPayConfig.TIME_EXPIRE; }
        //}

        public string notify_url
        {
            get
            {
                if (WxPayConfig.RECHARGE_NAME.Equals(this.body))
                {
                    return WxPayConfig.NOTIFY_RECHARGE_URL;
                }
                return WxPayConfig.NOTIFY_URL;
            }
        }

        public string trade_type {
            get { return WxPayConfig.TRADE_TYPE; }
        }

        public string openid { get; set; }

        public string product_id { get; set; }
    }
} 