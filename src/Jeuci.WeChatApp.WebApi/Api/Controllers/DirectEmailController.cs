using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using Abp.Web.Security.AntiForgery;
using Abp.WebApi.Controllers;
using Jeuci.WeChatApp.Api.Models;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.DirectEmail.Impl;
using Jeuci.WeChatApp.Filter;
using Jeuci.WeChatApp.InfrastructureServices.DirectEmail.Models;

namespace Jeuci.WeChatApp.Api.Controllers
{
    [RoutePrefix("api/directemail")]
    [JueciApiAuthorization]   
    public class DirectEmailController : AbpApiController
    {
        //private readonly DirectEmailAppService _directEmailAppService;

        //public DirectEmailController(DirectEmailAppService directEmailAppService)
        //{
        //    _directEmailAppService = directEmailAppService;
        //}

        //[HttpGet]
        //[Route("singlesendmail")]
        //public bool SingleSendMail([FromUri]SingleSendMailParams mailParams)
        //{
          
        //    var singleSendMailModel = new SingleSendMailModel(mailParams.ToAddress,mailParams.TmailBodyType, mailParams.EmailTempletService);
        //    return _directEmailAppService.SingleSendMail(singleSendMailModel);
        //}

        //[HttpGet]
        //[Route("batchsendmail")]
        //public bool BatchSendMail()
        //{

        //   // var singleSendMailModel = new SingleSendMailModel(mailParams.ToAddress, mailParams.TmailBodyType, mailParams.EmailTempletService);

        //    return false;
        //}
    }
}
