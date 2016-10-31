using Abp.Web.Mvc.Views;

namespace Jeuci.WeChatApp.Web.Views
{
    public abstract class WeChatAppWebViewPageBase : WeChatAppWebViewPageBase<dynamic>
    {

    }

    public abstract class WeChatAppWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected WeChatAppWebViewPageBase()
        {
            LocalizationSourceName = WeChatAppConsts.LocalizationSourceName;
        }
    }
}