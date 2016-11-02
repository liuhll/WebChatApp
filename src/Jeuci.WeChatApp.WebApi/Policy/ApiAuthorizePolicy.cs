using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Logging;
using Jeuci.WeChatApp.Common.Tools;

namespace Jeuci.WeChatApp.Policy
{
    public class ApiAuthorizePolicy : IApiAuthorizePolicy
    {
        private readonly Dictionary<string, string> _paramList;

        private readonly long _timestamp;

        private readonly string _sign;

        /// <summary>
        /// 授权策略
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="timestamp"></param>
        /// <param name="sign"></param>
        public ApiAuthorizePolicy(Dictionary<string, string> paramList, long timestamp, string sign)
        {
            _paramList = paramList;
            _timestamp = timestamp;
            _sign = sign;
        }

        public bool IsValidTime()
        {
            var now = DateTime.Now;
            var expirationDuration = ConfigHelper.GetIntValues("ExpirationDuration");
            var requestTime = DateTimeHelper.UnixTimestampToDateTime(_timestamp);
            if (requestTime.AddMinutes(expirationDuration) < now
                || requestTime.AddMinutes(-expirationDuration) > now)
            {
                LogHelper.Logger.Error("该请求不在有效的时效期");
                return false;
            }
            return true;
        }

        public bool IsLegalSign()
        {
            var saltKey = ConfigHelper.GetValuesByKey("SaltFigure");
            StringBuilder sb = new StringBuilder();
            var sortparamList = from objDic in _paramList orderby objDic.Key descending select objDic;
            foreach (var parm in sortparamList)
            {
                sb.Append(string.Format("{0}:{1}",parm.Key, parm.Value));
            }
            var isLegalSign = EncryptionHelper.EncryptSHA256(sb.ToString()).Equals(_sign);
            if (!isLegalSign)
            {
                LogHelper.Logger.Error("非法签名");
            }
            return isLegalSign;
        }
    }
}