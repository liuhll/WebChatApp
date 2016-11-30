using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.Mappings
{
    public class UserInfoMap : EntityTypeConfiguration<UserInfo>
    {
        public UserInfoMap()
        {
            ToTable("UserInfo");
            HasKey(t => t.Id);
        }
    }
}
