using System.Reflection;
using Abp.Modules;

namespace Jeuci.WeChatApp
{
    [DependsOn(typeof(WeChatAppCoreModule))]
    public class WeChatAppApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
