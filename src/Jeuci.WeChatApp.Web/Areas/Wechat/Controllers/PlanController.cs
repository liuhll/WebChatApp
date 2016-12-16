using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jeuci.WeChatApp.Web.Controllers;

namespace Jeuci.WeChatApp.Web.Areas.Wechat.Controllers
{
    public class PlanController : WeChatAppControllerBase
    {
        // GET: Wechat/Plan
        public ActionResult Index()
        {
            return View("~/App/Wechat/views/layout/planlayout.cshtml");
        }
    }
}