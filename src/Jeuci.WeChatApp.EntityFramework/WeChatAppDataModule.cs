using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using Jeuci.WeChatApp.EntityFramework;

namespace Jeuci.WeChatApp
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(WeChatAppCoreModule))]
    public class WeChatAppDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<WeChatAppDbContext>(null);
        }
    }
}
