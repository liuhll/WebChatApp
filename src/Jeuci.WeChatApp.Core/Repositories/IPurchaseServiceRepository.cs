using Abp.Domain.Repositories;
using Jeuci.WeChatApp.Pay.Models;

namespace Jeuci.WeChatApp.Repositories
{
    public interface IPurchaseServiceRepository : IRepository
    {
        bool GeneratePayOrder(PayOrder payOrder);

        int CompleteServiceOrder(CompleteServiceOrder payOrder);

        int FailServiceOrder(UpdateServiceOrder order);
    }
}