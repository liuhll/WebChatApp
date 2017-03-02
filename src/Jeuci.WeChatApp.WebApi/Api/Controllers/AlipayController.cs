using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using Abp.Logging;
using Abp.WebApi.Controllers;
using Jeuci.WeChatApp.Api.Models;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Pay.AliPay;
using Jeuci.WeChatApp.Pay.Lib;
using Jeuci.WeChatApp.Pay.Models;
using Jeuci.WeChatApp.Purchase;

namespace Jeuci.WeChatApp.Api.Controllers
{
    public class AlipayController : AbpApiController
    {
        private readonly IAlipayPurchaseService _alipayPurchaseService;

        public AlipayController(IAlipayPurchaseService alipayPurchaseAppService)
        {
            _alipayPurchaseService = alipayPurchaseAppService;
        }

        [HttpPost]
        public HttpResponseMessage WapPay()
        {
            LogHelper.Logger.Debug("回调支付宝支付接口");
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var alipayData = new AlipayData();
           
            foreach (var key in HttpContext.Current.Request.Form.AllKeys)
            {               
                alipayData.SetValue(key, HttpContext.Current.Request.Form[key]);
            }
            var signVerify = alipayData.SignVerified();
            if (!signVerify)
            {
                Logger.Error("签名失败，这可能是假冒的回调");
                response.Content = new StringContent("fail", Encoding.UTF8, "text/html");
                return response;
            }

            response.Content = _alipayPurchaseService.AlipayCallBack(alipayData) ? new StringContent("success", Encoding.UTF8, "text/html") : new StringContent("fail", Encoding.UTF8, "text/html");
            return response;
        }
              
    }
}