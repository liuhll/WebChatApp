using System.Threading.Tasks;
using Abp.Dependency;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.Wechat.Accounts
{
    public interface IBindAccount : ITransientDependency
    {
        Task<ResultMessage<bool>> BindWechatAccount(JeuciAccount input);
    }
}