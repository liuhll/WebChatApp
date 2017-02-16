using Abp.Dependency;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Pay.Lib;

namespace Jeuci.WeChatApp.Pay
{
    public interface IOrderPolicy : ITransientDependency
    {
        WxPayData Orderquery(string orderId, OrderType type);
    }
}