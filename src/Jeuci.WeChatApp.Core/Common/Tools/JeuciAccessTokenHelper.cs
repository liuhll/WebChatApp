using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeuci.WeChatApp.Common.Tools
{
    public static class JeuciAccessTokenHelper
    {
        public static string GetSignStr(IDictionary<string, string> mParamsDict)
        {
            var saltKey = ConfigHelper.GetValuesByKey("EmaliServiceToken");
            StringBuilder sb = new StringBuilder();
            var sortparamList = from objDic in mParamsDict orderby objDic.Key descending select objDic;
            foreach (var parm in sortparamList)
            {
                sb.Append(string.Format("{0}:{1}", parm.Key.ToLower(), parm.Value));
            }
            var encryptedStr = sb.ToString() + saltKey;
            return EncryptionHelper.EncryptSHA256(encryptedStr);
        }
    }
}
