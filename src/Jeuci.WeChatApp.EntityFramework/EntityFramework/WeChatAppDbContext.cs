using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using Abp.EntityFramework;
using Jeuci.WeChatApp.Lottery.Models;
using Jeuci.WeChatApp.Mappings;
using Jeuci.WeChatApp.Wechat.Models.Account;
using Jeuci.WeChatApp.Wechat.Models.Message;

namespace Jeuci.WeChatApp.EntityFramework
{
    public class WeChatAppDbContext : AbpDbContext
    {
        //TODO: Define an IDbSet for each Entity...

        public virtual IDbSet<UserInfo> UserInfos { get; set; }

        public virtual IDbSet<LotteryPlanLib> LotteryPlanLibs { get; set; }

        public virtual IDbSet<WechatMsg> WechatMsgs { get; set; }

        public virtual IDbSet<ServiceInfo> ServiceInfos { get; set; }

        //Example:
        //public virtual IDbSet<User> Users { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public WeChatAppDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in WeChatAppDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of WeChatAppDbContext since ABP automatically handles it.
         */
        public WeChatAppDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserInfoMap());
            modelBuilder.Configurations.Add(new LotteryPlanLibMap());
            modelBuilder.Configurations.Add(new WechatMsgMap());
            modelBuilder.Configurations.Add(new ServiceInfoMap());
        }
    }
}
