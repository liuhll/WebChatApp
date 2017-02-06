using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jeuci.WeChatApp.Web.Areas.Wechat.Controllers
{
    public class PurchaseController : Controller
    {
        // GET: Wechat/Purchase
        public ActionResult Index()
        {
            return View("~/App/Wechat/views/layout/planlayout.cshtml");
        }
    }
}