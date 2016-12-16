using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Jeuci.WeChatApp.Lottery.Models;

namespace Jeuci.WeChatApp.Lottery.Server
{
    public interface ILotteryServer : ITransientDependency
    {
        IList<ServerList> GetServiceList();

        IList<PlanInfo> GetPlanList(int sid, out string msg);
    }
}
