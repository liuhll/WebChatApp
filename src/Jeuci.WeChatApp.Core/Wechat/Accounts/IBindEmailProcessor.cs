using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Jeuci.WeChatApp.Wechat.Models.BindEmail;

namespace Jeuci.WeChatApp.Wechat.Accounts
{
    public interface IBindEmailProcessor : ITransientDependency
    {
        Task<bool> SendValidByEmail(string openId, string emailAddress);

        bool BindUserEmail(BindEmailModel model, out string msgOrUrl);
    }
}
