﻿using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Jeuci.WeChatApp.Web
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //ASP.NET Web API Route Config
            //routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //    );

            routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }             
            );

          //  routes.MapRoute(
          //    "Wechat",
          //    "Wechat/{controller}/{action}",
          //    new { controller = "Home", action = "Index",id = UrlParameter.Optional },
          //    new string[] { "Jeuci.WeChatApp.Web.Areas.Wechat.Controllers" }
          //);
        }
    }
}
