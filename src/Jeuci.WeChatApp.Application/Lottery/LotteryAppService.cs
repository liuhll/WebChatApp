using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Lottery.Models;
using Jeuci.WeChatApp.Lottery.Server;

namespace Jeuci.WeChatApp.Lottery
{
    public class LotteryAppService : ILotteryAppService
    {
        private readonly ILotteryServer _lotteryServer;

        public LotteryAppService(ILotteryServer lotteryServer)
        {
            _lotteryServer = lotteryServer;
        }

        public ResultMessage<IList<ServerList>> GetServiceList()
        {
            try
            {
                var result =  _lotteryServer.GetServiceList();
                return new ResultMessage<IList<ServerList>>(result);
            }
            catch (Exception e)
            {
                return new ResultMessage<IList<ServerList>>(ResultCode.Fail,e.Message);
            }
        }

        public ResultMessage<IList<PlanInfo>> PlanList(int sid)
        {
            string msg;
            try
            {
                var result = _lotteryServer.GetPlanList(sid,out msg);
                return result == null ? new ResultMessage<IList<PlanInfo>>(ResultCode.Fail,msg)
                    : new ResultMessage<IList<PlanInfo>>(result);
            }
            catch (Exception e)
            {
                return new ResultMessage<IList<PlanInfo>>(ResultCode.Fail, e.Message);
            }
            

        }
    }
}