using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Email.Dtos;

namespace Jeuci.WeChatApp.Email
{
    public interface IBindEmailAppService : IApplicationService
    {
        //bool SingleSendMail(SingleSendMailModel model);

        [HttpGet]
        Task<ResultMessage<string>> GetValidCodeByEmail(string openId,string emailAddress);

        [HttpPost]
        ResultMessage<string> BindUserEmail(BindEmailInput input);
    }
}
