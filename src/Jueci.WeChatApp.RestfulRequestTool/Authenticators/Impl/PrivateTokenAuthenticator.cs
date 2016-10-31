using RestSharp;

namespace Jueci.WeChatApp.RestfulRequestTool.Authenticators.Impl
{
    public class PrivateTokenAuthenticator : IPrivateTokenAuthenticator
    {
        /// <summary> The user's private token. </summary>
        public string PrivateToken { get; set; }

        public PrivateTokenAuthenticator(string privateToken)
        {
            PrivateToken = privateToken;
        }
        /// <summary> Adds the provided private token to the request as a header. </summary>
        /// <param name="client"> The <see cref="IRestClient" />. </param>
        /// <param name="request"> The <see cref="IRestRequest" /> to add the header to. </param>
        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddParameter("PRIVATE-TOKEN", PrivateToken, ParameterType.HttpHeader);
        }
    }
}