using Abp.Dependency;

namespace Jeuci.WeChatApp.Pay
{
    public interface IOrderExecutor : ISingletonDependency
    {
        bool UpdateServiceOrder();
    }
}