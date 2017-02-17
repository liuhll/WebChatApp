using System.Collections;
using System.Collections.Generic;
using Abp.Dependency;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Pay.Lib;
using Jeuci.WeChatApp.Pay.Models;

namespace Jeuci.WeChatApp.Pay
{
    public interface IPurchaseService : ITransientDependency
    {
        WxPayData UnifiedOrderResult(ServiceOrder serviceOrder, int timeOut = 6);

        WxpayOptions GetWxpayOptions(string nonceStr,string url);

        PayOptions GetPaySign(string package, string nonceStr);

        bool GeneratePayOrder(PayOrder payOrder,out string msg);

        int CompleteServiceOrder(WxPayData payData);

        void FailServiceOrder(UpdateServiceOrder order);

        int ClientCompleteServiceOrder(CompleteServiceOrder mapTo);

        IList<UserPayOrderInfo> GetNeedQueryOrderList(PayMode mmobileWeb);
    }
}