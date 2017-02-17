using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Jeuci.WeChatApp.Pay;
using Jeuci.WeChatApp.Purchase;

namespace Jeuci.WeChatApp
{
    [DependsOn(typeof(WeChatAppCoreModule), typeof(AbpAutoMapperModule))]
    public class WeChatAppApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
      
    }
}
