using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeuci.WeChatApp.Wechat.Models.Message;

namespace Jeuci.WeChatApp.Mappings
{
    public class WechatMsgMap : EntityTypeConfiguration<WechatMsg>
    {
        public WechatMsgMap()
        {
            ToTable("WechatMsg");
            HasKey(p => p.Id);
        }
    }
}
