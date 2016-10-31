using Abp.Web.Mvc.Controllers;

namespace Jeuci.WeChatApp.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class WeChatAppControllerBase : AbpController
    {
        protected WeChatAppControllerBase()
        {
            LocalizationSourceName = WeChatAppConsts.LocalizationSourceName;
        }
    }
}