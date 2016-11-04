using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using Abp.Json;
using Abp.WebApi.Controllers;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.WeChatAuth;
using Jeuci.WeChatApp.WeChatAuth.Dtos;
using Newtonsoft.Json;
using Senparc.Weixin.MP.Entities.Request;
using WechatSdk = Senparc.Weixin.MP;

namespace Jeuci.WeChatApp.Api.Controllers
{
    [RoutePrefix("api/WechatSign")]
    public class WechatController : AbpApiController
    {
        private readonly IWechatAuthAppService _wechatAuthAppService;

        public WechatController(IWechatAuthAppService wechatAuthAppService)
        {
            _wechatAuthAppService = wechatAuthAppService;
        }

        [HttpGet]
        public HttpResponseMessage Index([FromUri] WechatSignInput signParams)
        {   
            Logger.Info("请求的头：" + JsonConvert.SerializeObject(Request.Headers));
            Logger.Info("请求的Content：" + JsonConvert.SerializeObject(Request.Content));
            Logger.Info("请求的URL" + JsonConvert.SerializeObject(Request.RequestUri));
           
            var ip = ((HttpContextBase)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;

            Logger.Info("请求的Ip:" + ip);
            if (_wechatAuthAppService.CheckSignature(signParams))
            {
                var res = Request.CreateResponse(HttpStatusCode.OK, signParams.Echostr);
                res.Content = new StringContent(signParams.Echostr, Encoding.UTF8, "text/html");
                return res;
            }
            return Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, string.Empty);
        }


        //[HttpPost]
        //public bool Index(PostModel postModel)
        //{
        //    Logger.Info("-------------------Post请求------------------------");
        //    Logger.Info("请求的头：" + JsonConvert.SerializeObject(Request.Headers));
        //    Logger.Info("请求的Content：" + JsonConvert.SerializeObject(Request.Content));
        //    Logger.Info("请求的URL" + JsonConvert.SerializeObject(Request.RequestUri));


        //    if (!WechatSdk.CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
        //    {
        //        return false;
        //    }
        //    return true;
         
        //}


    }
}