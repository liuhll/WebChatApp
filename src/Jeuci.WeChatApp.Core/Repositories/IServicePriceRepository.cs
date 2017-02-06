using System.Collections.Generic;
using Abp.Domain.Repositories;
using Jeuci.WeChatApp.Lottery.Models;

namespace Jeuci.WeChatApp.Repositories
{
    public interface IServicePriceRepository : IRepository<ServerPrice>
    {
        IList<ServerPrice> GetServerPrices(int sid, int userId);
    }
}
