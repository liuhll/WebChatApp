using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;
using Jueci.WeChatApp.RestfulRequestTool.Common.Enums;
using Jueci.WeChatApp.RestfulRequestTool.RequestAbstractions;
using Jueci.WeChatApp.RestfulRequestTool.RequestAbstractions.Impl;

namespace Jeuci.WeChatApp.Services.Email
{
    public class DirectEmailService :　IDirectEmailService
    {
        private readonly string m_emailService;
     
        private readonly IRequestFactory m_requestFactory;

        private readonly IDictionary<string, string> m_paramsDict;

        public DirectEmailService()
        {
            m_emailService = ConfigHelper.GetValuesByKey("DirectEmailService");
            m_requestFactory = new RequestFactory(m_emailService);
            m_paramsDict = new Dictionary<string, string>();
            m_paramsDict.Add("subject","彩盟网邮箱绑定验证码");

        }

        public async Task<bool> SendValidCodeByEmail(string emailAddresss, string body)
        {
            m_paramsDict.Add("emailBody", body);
            m_paramsDict.Add("toAddress", emailAddresss);
            m_paramsDict.Add("timestamp", DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString());
            
            var request = m_requestFactory.Create("/api/DirectEmail/SingleSendMailByBody", Method.Get, false);

            foreach (var item in m_paramsDict)
            {
                request.AddParameter("mailParams." + item.Key.ToLower(),item.Value);
            }

            request.AddParameter("mailParams." + "sign",JeuciAccessTokenHelper.GetSignStr(m_paramsDict));

            var result = await request.Execute<DirectEmailMessage>();

            return result.StatusCode == HttpStatusCode.OK && result.Data.Code == ResultCode.Success ;
        }
    }
}
