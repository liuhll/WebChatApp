using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Jeuci.WeChatApp.InfrastructureServices.DirectEmail.Models;

namespace Jeuci.WeChatApp.DirectEmail
{
    public interface IDirectEmailAppService : ITransientDependency
    {
        //bool SingleSendMail(SingleSendMailModel model);
    }
}
