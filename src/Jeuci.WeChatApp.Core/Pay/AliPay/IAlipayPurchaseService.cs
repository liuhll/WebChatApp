using Abp.Dependency;
using Jeuci.WeChatApp.Pay.Lib;
using Jeuci.WeChatApp.Pay.Models;

namespace Jeuci.WeChatApp.Pay.AliPay
{
    public interface IAlipayPurchaseService : ITransientDependency
    {
        bool PayOrder(AliPayOrder input, out string msg);

        bool AlipayCallBack(AlipayData alipayData);
        void UpdateAliPayOrder(AlipayData alipayData);
    }
}