using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Jeuci.WeChatApp.Pay.Lib;
using Jeuci.WeChatApp.Pay.Models;

namespace Jeuci.WeChatApp.Pay
{
    public interface IRechargeService : ITransientDependency
    {
        IList<int> GetFreeList();

        bool CompleteRechargeOrder(WxPayData payData,out string msg);
        bool ClientCompleteServiceOrder(CompleteServiceOrder mapTo, out string msg);
    }
}
