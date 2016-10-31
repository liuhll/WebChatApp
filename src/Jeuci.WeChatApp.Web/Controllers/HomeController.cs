using System.Web.Mvc;

namespace Jeuci.WeChatApp.Web.Controllers
{
    public class HomeController : WeChatAppControllerBase
    {
        public ActionResult Index()
        { 
            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
	}
}