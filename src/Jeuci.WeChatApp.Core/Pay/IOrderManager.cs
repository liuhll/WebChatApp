using Abp.Dependency;

namespace Jeuci.WeChatApp.Pay
{
    public interface IOrderManager : ITransientDependency
    {
        void Start();
    }
}