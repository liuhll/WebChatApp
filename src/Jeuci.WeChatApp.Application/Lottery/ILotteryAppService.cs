using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Lottery.Models;
using Jeuci.WeChatApp.Lottery.Server;

namespace Jeuci.WeChatApp.Lottery
{
    public interface ILotteryAppService : IApplicationService
    {
        ResultMessage<IList<ServerList>> GetServiceList();

        [HttpGet]
        ResultMessage<IList<PlanInfo>> PlanList(int sid);
    }
}