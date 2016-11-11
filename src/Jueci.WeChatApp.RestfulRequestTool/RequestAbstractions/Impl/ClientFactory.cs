using System;
using Jueci.WeChatApp.RestfulRequestTool.Authenticators;
using RestSharp;

namespace Jueci.WeChatApp.RestfulRequestTool.RequestAbstractions.Impl
{
    internal class ClientFactory : IClientFactory
    {

        private readonly IPrivateTokenAuthenticator _authenticator;

        private readonly Uri _baseUri;

        public ClientFactory(Uri baseUri, IPrivateTokenAuthenticator authenticator)
        {
            _baseUri = baseUri;
            _authenticator = authenticator;
        }

        public ClientFactory(string baseUri)
        {
            _baseUri = new Uri(baseUri);
        }

        public IRestClient Create(bool authenticate = true)
        {
            if (_authenticator == null)
            {
                authenticate = false;
            }
            var client = new RestClient(_baseUri);
            if (authenticate)
            {
                client.Authenticator = _authenticator;
            }
            return client;
        }
    }
}
