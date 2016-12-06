using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Dependency;
using Jeuci.WeChatApp.Common;

namespace Jeuci.WeChatApp.Email
{
    public interface IBindEmailAppService : IApplicationService
    {
        //bool SingleSendMail(SingleSendMailModel model);

        [HttpGet]
        Task<ResultMessage<string>> GetValidCodeByEmail(string openId,string emailAddress);
    }
}
