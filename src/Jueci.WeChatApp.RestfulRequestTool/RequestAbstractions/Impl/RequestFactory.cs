using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Logging;
using Jueci.WeChatApp.RestfulRequestTool.Authenticators.Impl;
using Jueci.WeChatApp.RestfulRequestTool.Common;
using RestSharp;
using Method = Jueci.WeChatApp.RestfulRequestTool.Common.Enums.Method;

namespace Jueci.WeChatApp.RestfulRequestTool.RequestAbstractions.Impl
{
    public class RequestFactory : IRequestFactory
    {
        private readonly IClientFactory _clientFactory;

        public RequestFactory(IClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public RequestFactory(string baseUri)
        {
            _clientFactory = new ClientFactory(baseUri);
        }

        public IRequest Create(string resource, Method method, bool authenticate = true)
        {
            var client = _clientFactory.Create(authenticate);
            return new Request(resource, method, client);
        }

        public IRequest Create(string resource, Method method, string accessToken,string accessTokenName = "access_token")
        {
            var client = _clientFactory.Create();
            var request = new Request(resource,method,client);
            if (string.IsNullOrEmpty(accessToken))
            {
               // LogHelper.Logger.Error("accessToken不允许为空");
                throw new RequestException("accessToken 不允许为空");
            }
            request.AddParameter(accessTokenName,accessToken);
            return request;
        }


    }
}
