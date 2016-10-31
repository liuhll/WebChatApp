using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Method = Jueci.WeChatApp.RestfulRequestTool.Common.Enums.Method;

namespace Jueci.WeChatApp.RestfulRequestTool.RequestAbstractions.Impl
{
    internal class RequestFactory : IRequestFactory
    {
        private readonly IClientFactory _clientFactory;

        public RequestFactory(IClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IRequest Create(string resource, Method method, bool authenticate = true)
        {
            var client = _clientFactory.Create(authenticate);

            return new Request(resource, method, client);
        }
    }
}
