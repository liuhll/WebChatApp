using RestSharp.Authenticators;

namespace Jueci.WeChatApp.RestfulRequestTool.Authenticators
{
    public interface IPrivateTokenAuthenticator : IAuthenticator
    {
        string PrivateToken { get; set; }
    }
}