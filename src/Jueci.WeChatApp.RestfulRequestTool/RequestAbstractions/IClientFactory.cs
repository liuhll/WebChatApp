using Abp.Dependency;
using RestSharp;

namespace Jueci.WeChatApp.RestfulRequestTool.RequestAbstractions
{
    internal interface IClientFactory : ITransientDependency
    {
        IRestClient Create(bool authenticate = true);
    }
}