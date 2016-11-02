using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Abp.Collections.Extensions;
using Abp.Logging;
using Jeuci.WeChatApp.Common.Exceptions;
using Jeuci.WeChatApp.Policy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jeuci.WeChatApp.Filter
{
    public class JueciApiAuthorization : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
#if DEBUG            
            return true;
#else
            string sign;
            long timestamp;
            var paramList = GetRequestParams(actionContext,out sign,out timestamp);
            IApiAuthorizePolicy authorizePolicy = new ApiAuthorizePolicy(paramList,timestamp,sign);
            return authorizePolicy.IsValidTime() && authorizePolicy.IsLegalSign();
            
#endif
        }

        private Dictionary<string,string> GetRequestParams(HttpActionContext actionContext, out string sign, out long timestamp)
        {
            var requestParams = new Dictionary<string, string>();
            var requestSign = string.Empty;
            long requesTimestamp = 0;
            if (actionContext.Request.Method == HttpMethod.Get)
            {
                var qString = actionContext.Request.RequestUri.ParseQueryString();
                //Array.Sort(qString.AllKeys);
                foreach (var q in qString.AllKeys)
                {
                    if (q.ToLower().Contains("sign"))
                    {
                        requestSign = qString[q];
                    }
                    if (q.ToLower().Contains("timestamp"))
                    {
                        requesTimestamp = Convert.ToInt64(qString[q]);
                    }
                    requestParams.Add(q.Split('.')[1].ToLower(), qString[q]);
                }
            }
            else
            {
                //actionContext.Request.Content
                Task<string> content = actionContext.Request.Content.ReadAsStringAsync();
                string body = content.Result;
                var data = (JObject)JsonConvert.DeserializeObject(body);
                foreach (var item in data)
                {
                    if (item.Key.Equals("sign", StringComparison.OrdinalIgnoreCase))
                    {
                        requestSign = item.Value.ToString();
                    }
                    if (item.Key.Equals("timestamp",StringComparison.OrdinalIgnoreCase))
                    {
                        requesTimestamp = Convert.ToInt64(item.Value.ToString());
                    }

                    requestParams.Add(item.Key.ToLower(),item.Value.ToString());
                }
            }
            if (string.IsNullOrEmpty(requestSign))
            {
                LogHelper.Logger.Error("无效的请求，该请求没有签名，无法通过验证");
                throw new ApiAuthorizeException("请求的签名不能为空");
            }
            if (requesTimestamp == 0)
            {
                LogHelper.Logger.Error("无效的请求，没有时间戳，无法通过验证");
                throw new ApiAuthorizeException("请求的时间戳不能为空");
            }

            sign = requestSign;
            timestamp = requesTimestamp;
            
            return requestParams;
        }
    }
}