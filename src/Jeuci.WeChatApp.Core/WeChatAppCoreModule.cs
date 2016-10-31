using System.Reflection;
using Abp.Modules;
using Jueci.WeChatApp.RestfulRequestTool;

namespace Jeuci.WeChatApp
{
    [DependsOn(typeof(RestfulRequestToolModule))]
    public class WeChatAppCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
