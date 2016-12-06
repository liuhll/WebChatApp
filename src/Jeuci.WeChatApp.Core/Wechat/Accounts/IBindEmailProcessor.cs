using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;

namespace Jeuci.WeChatApp.Wechat.Accounts
{
    public interface IBindEmailProcessor : ITransientDependency
    {
        Task<bool> SendValidByEmail(string openId, string emailAddress);
    }
}
