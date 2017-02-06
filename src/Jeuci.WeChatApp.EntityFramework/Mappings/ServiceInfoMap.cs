using System.Data.Entity.ModelConfiguration;
using Jeuci.WeChatApp.Lottery.Models;

namespace Jeuci.WeChatApp.Mappings
{
    public class ServiceInfoMap : EntityTypeConfiguration<ServiceInfo>
    {
        public ServiceInfoMap()
        {
            ToTable("ServiceInfo");
        }
    }
}