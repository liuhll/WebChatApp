using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;

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
