using Abp.Application.Services;

namespace Jeuci.WeChatApp
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class WeChatAppAppServiceBase : ApplicationService
    {
        protected WeChatAppAppServiceBase()
        {
            LocalizationSourceName = WeChatAppConsts.LocalizationSourceName;
        }
    }
}