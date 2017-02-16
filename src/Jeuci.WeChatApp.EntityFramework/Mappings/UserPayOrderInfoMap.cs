using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeuci.WeChatApp.Pay.Models;

namespace Jeuci.WeChatApp.Mappings
{
    public class UserPayOrderInfoMap : EntityTypeConfiguration<UserPayOrderInfo>
    {
        public UserPayOrderInfoMap()
        {
            ToTable("UserPayOrderInfo");
        }
    }
}
