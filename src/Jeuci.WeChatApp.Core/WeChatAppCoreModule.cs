using System.Reflection;
using Abp.Modules;

namespace Jeuci.WeChatApp
{
    public class WeChatAppCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
