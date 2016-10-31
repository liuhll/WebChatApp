using System.Web.Http;
using Abp.WebApi.Controllers;
using Jeuci.WeChatApp.Api.Models;

namespace Jeuci.WeChatApp.Api.Controllers
{
    [RoutePrefix("api/WechatSign")]
    public class WechatSignatureController : AbpApiController
    {
        [HttpGet]
        public bool CheckSignature([FromUri] SignParams signParams)
        {
            return true;
        }
    }
}