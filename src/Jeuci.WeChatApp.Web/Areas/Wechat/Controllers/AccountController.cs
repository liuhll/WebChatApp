using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jeuci.WeChatApp.Web.Controllers;

namespace Jeuci.WeChatApp.Web.Areas.Wechat.Controllers
{
    public class AccountController : WeChatAppControllerBase
    {
    
        public ActionResult Index()
        {
            return View("~/App/Wechat/views/layout/layout.cshtml");
        }
    }
}