using Abp.Dependency;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Pay.Lib;
using Jeuci.WeChatApp.Pay.Models;

namespace Jeuci.WeChatApp.Pay.AliPay
{
    public interface IAlipayRequest : ISingletonDependency
    {
        bool Wappay(AlipayOrderOptions options, out string msg);

        AlipayData Query(string id, OrderType outTradeNo);
    }
}