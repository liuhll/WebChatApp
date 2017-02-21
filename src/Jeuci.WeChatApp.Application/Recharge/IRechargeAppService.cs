using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Pay.Lib;
using Jeuci.WeChatApp.Purchase.Dtos;

namespace Jeuci.WeChatApp.Recharge
{
    public interface IRechargeAppService : IApplicationService
    {
        ResultMessage<IList<int>> GetFreeList();

        ResultMessage<ServiceInfoOutput> GetUnifiedOrder(string openId,double fee);

        ResultMessage<string> CompleteRechargeOrder(WxPayData payData);

        ResultMessage<string> ClientCompleteServiceOrder(PayOrderDto payOrder);
    }
}