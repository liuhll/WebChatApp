using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using Abp.Domain.Repositories;
using Abp.Logging;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Lottery.Models;
using Jeuci.WeChatApp.Lottery.Tools;
using Jueci.WeChatApp.RestfulRequestTool.RequestAbstractions;
using Jueci.WeChatApp.RestfulRequestTool.RequestAbstractions.Impl;

namespace Jeuci.WeChatApp.Lottery.Server
{

    public class LotteryServer : ILotteryServer
    {
        private string m_lotteryServiceAddress;
       // private IRequestFactory _requestFactory;
        private int _freePlanNum;
        private IRepository<LotteryPlanLib,string> _lotteryPlanLibRepository;
        private string m_planNamePrefix = "免费计划";
        private readonly string m_planServiceAddress;


        public LotteryServer(IRepository<LotteryPlanLib,string> lotteryPlanLibRepository)
        {
            _lotteryPlanLibRepository = lotteryPlanLibRepository;
            m_lotteryServiceAddress = ConfigHelper.GetValuesByKey("LotteryServiceAddress");
            _freePlanNum = ConfigHelper.GetIntValues("FreePlanNum");
          //  _requestFactory = new RequestFactory(m_lotteryServiceAddress);
            m_planServiceAddress = ConfigHelper.GetValuesByKey("PlanServiceAddress");

        }

        public IList<ServerList> GetServiceList()
        {

            var paramsList = new NameValueCollection()
            {
                { "Action","GetServiceList"}
            };

            var webClient = new WebClient();
            var responseData =  webClient.UploadValues(m_lotteryServiceAddress,"POST", paramsList);

            var result = Encoding.UTF8.GetString(responseData);//解码

            if (string.IsNullOrEmpty(result))
            {
                throw new Exception("获取彩票类型失败");
            }
            var obj = result.FromJson<LotteryServerResultMessage<List<ServerList>>>();
            return obj.Data;
        }

        public IList<PlanInfo> GetPlanList(int sid, out string msg)
        {
            var cptypeInfo = CpTypeSIdMapTool.GeTypeSIdMaps(sid);

            var freePlanLibs =
                _lotteryPlanLibRepository.GetAllList(p => p.SId == sid 
                && p.State != 3 
                && string.IsNullOrEmpty(p.VCode))
                    .OrderByDescending(p => p.State)
                    .ThenByDescending(p => p.CreateTime).Take(_freePlanNum).ToList();

            if (freePlanLibs.Count <= 0)
            {
               LogHelper.Logger.Error("用户尝试获取sid为" + sid +"的计划，但是计划库并没有改彩种的有效计划");
                throw new Exception("计划库中没有存在该彩种的计划，请稍后再尝试获取!");
            }

            var freePlanInfos = new List<PlanInfo>();
            var count = 1;
            foreach (var planlib in freePlanLibs)
            {
                var planInfo = new PlanInfo()
                {
                    PlanName = m_planNamePrefix + count,
                    PlanUrl = string.Format("{0}/app/{1}/planshare/{2}", m_planServiceAddress, cptypeInfo.CpType,planlib.Id)
                };
                count ++;
                freePlanInfos.Add(planInfo);
            }
            msg = "OK";
            return freePlanInfos;
        }
    }
}