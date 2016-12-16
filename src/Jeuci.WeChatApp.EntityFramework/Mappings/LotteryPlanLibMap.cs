using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeuci.WeChatApp.Lottery.Models;

namespace Jeuci.WeChatApp.Mappings
{
    public class LotteryPlanLibMap : EntityTypeConfiguration<LotteryPlanLib>
    {
        public LotteryPlanLibMap()
        {

            ToTable("LotteryPlanLib");
            HasKey(t => t.Id);
        }
    }
}
