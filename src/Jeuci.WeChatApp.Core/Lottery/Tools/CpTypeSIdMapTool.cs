using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Logging;
using Camew.Lottery;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Lottery.Models;

namespace Jeuci.WeChatApp.Lottery.Tools
{
    internal class CpTypeSIdMapTool
    {
        private static IList<CpTypeSIdMap> _cpTypeSIdMaps
        {
            get
            {

                var cptypeSidMapsStr = Regex.Replace(ConfigHelper.GetValuesByKey("CpTypeSIdMap"), @"\s", "");
                return cptypeSidMapsStr.FromJson<List<CpTypeSIdMap>>();
                
            }
        }



        public static CpTypeSIdMap GeTypeSIdMaps(int sid)
        {
            if (_cpTypeSIdMaps == null )
            {
                LogHelper.Logger.Error("获取彩票类型与Sid映射关系失败，原因：可能还没有相关配置");
                throw new Exception("获取彩票类型与Sid映射关系失败");
            }
            if (_cpTypeSIdMaps.All(p => p.SId != sid))
            {
                LogHelper.Logger.Error("服务器还没有配置与服务类型相关的彩种");
                throw new Exception("服务器还没有配置与服务类型相关的彩种");
            }
            return _cpTypeSIdMaps.First(p => p.SId == sid);
        }
    }
}
