using Jeuci.WeChatApp.InfrastructureServices.DirectEmail;
using Jeuci.WeChatApp.InfrastructureServices.DirectEmail.Models;

namespace Jeuci.WeChatApp.DirectEmail.Impl
{
    public class DirectEmailAppService : IDirectEmailAppService
    {
        private readonly IDirectEmailService _directEmailService;

        public DirectEmailAppService(IDirectEmailService directEmailService)
        {
            _directEmailService = directEmailService;
        }

        public bool SingleSendMail(SingleSendMailModel model)
        {
            return _directEmailService.SingleSendMail(model);
        }
    }
}