using System.Reflection;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;

namespace Jeuci.WeChatApp
{
    [DependsOn(typeof(AbpWebApiModule), typeof(WeChatAppApplicationModule), typeof(AbpAutoMapperModule))]
    public class WeChatAppWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(WeChatAppApplicationModule).Assembly, "app")
                .Build();
        }
    }
}
