using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Jeuci.WeChatApp.Configs;
using Jueci.WeChatApp.RestfulRequestTool;

namespace Jeuci.WeChatApp
{
    [DependsOn(typeof(RestfulRequestToolModule), typeof(AbpAutoMapperModule))]
    public class WeChatAppCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            ModelMapperConfiguration.ConfigModelMapper();
            base.PreInitialize();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
