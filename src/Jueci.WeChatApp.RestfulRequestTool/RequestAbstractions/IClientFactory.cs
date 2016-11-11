using Abp.Dependency;
using RestSharp;

namespace Jueci.WeChatApp.RestfulRequestTool.RequestAbstractions
{
    public interface IClientFactory : ITransientDependency
    {
        IRestClient Create(bool authenticate = true);
    }
}