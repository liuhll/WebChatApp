using System.Collections.Generic;
using Aop.Api.Util;
using LitJson;

namespace Jeuci.WeChatApp.Pay.Lib
{
    public class AlipayData
    {

        private IDictionary<string, string> m_values = new Dictionary<string, string>();


        public AlipayData()
        {

        }


        /**
    * 设置某个字段的值
    * @param key 字段名
     * @param value 字段值
    */
        public void SetValue(string key, string value)
        {
            m_values[key] = value;
        }

        /**
        * 根据字段名获取某个字段的值
        * @param key 字段名
         * @return key对应的字段值
        */
        public string GetValue(string key)
        {
            string o = null;
            m_values.TryGetValue(key, out o);
            return o;
        }

        public bool IsSet(string key)
        {
            string o = null;
            m_values.TryGetValue(key, out o);
            if (null != o)
                return true;
            else
                return false;
        }

        public bool SignVerified()
        {
            return AlipaySignature.RSACheckV1(m_values, AliPayConfig.ALIPAY_PUBLIC_KEY, AliPayConfig.CHARSET, AliPayConfig.SIGN_TYPE, false);
        }

        public string ToJson()
        {
            string jsonStr = JsonMapper.ToJson(m_values);
            return jsonStr;
        }
    }
}