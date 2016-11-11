using System.Web.Mvc;

namespace Jeuci.WeChatApp.Web.Areas.Wechat
{
    public class WechatAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Wechat";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Wechat_default",
                "Wechat/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}