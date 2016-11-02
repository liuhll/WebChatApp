using Abp.Dependency;
using Jeuci.WeChatApp.InfrastructureServices.DirectEmail.Models;

namespace Jeuci.WeChatApp.InfrastructureServices.DirectEmail
{
    public interface IDirectEmailService : ITransientDependency
    {
        bool SingleSendMail(SingleSendMailModel model);

        bool BatchSendMail();
    }
}