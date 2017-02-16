using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;
using Abp.Json;
using Abp.WebApi.Controllers;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.WechatMsg;
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
        private readonly IWechatMsgAppService _wechatMsgAppService;

        public WechatController(IWechatAuthAppService wechatAuthAppService,
            IWechatMsgAppService wechatMsgAppService)
        {
            _wechatAuthAppService = wechatAuthAppService;
            _wechatMsgAppService = wechatMsgAppService;
        }

        [HttpGet]
        public HttpResponseMessage Index([FromUri] WechatSignInput signParams)
        {   
            if (_wechatAuthAppService.CheckSignature(signParams))
            {
                var res = Request.CreateResponse(HttpStatusCode.OK, signParams.Echostr);
                res.Content = new StringContent(signParams.Echostr, Encoding.UTF8, "text/html");
                return res;
            }
            return Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, string.Empty);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Index()
        {
            var stream = await Request.Content.ReadAsStreamAsync();
            XDocument doc = XDocument.Load(stream);
            var requestMessage = WechatSdk.RequestMessageFactory.GetRequestEntity(doc);            
            var res = Request.CreateResponse(HttpStatusCode.OK);
            //string msg = "<xml>" +
            //             "<ToUserName><![CDATA[{0}]]></ToUserName>" +
            //             "<FromUserName><![CDATA[camew-com]]></FromUserName>" +
            //             "<CreateTime>{1}</CreateTime>" +
            //             "<MsgType><![CDATA[text]]></MsgType>" +
            //             "<Content><![CDATA[{2}]]></Content>" +
            //             "</xml>";
            string msg = string.Empty;
            try
            {
                msg = _wechatMsgAppService.MsgHandlerByRequestContent(requestMessage);
                res.Content = new StringContent(msg, Encoding.UTF8, "text/xml");
            }
            catch (Exception e)
            {
                Logger.Error("不支持的处理消息类型" + e.Message);
                msg = "success";
                res.Content = new StringContent(msg, Encoding.UTF8);
            }
           
            return res;
        }

        //[HttpPost]
        //public bool Index(PostModel postModel)
        //{
        //    Logger.InfosuccessPost请求------------------------");
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