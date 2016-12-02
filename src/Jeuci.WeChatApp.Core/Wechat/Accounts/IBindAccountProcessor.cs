using System.Threading.Tasks;
using Abp.Dependency;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.Wechat.Accounts
{
    public interface IBindAccountProcessor : ITransientDependency
    {
        bool BindWechatAccount(JeuciAccount input, out string urlOrMsg);
        bool UnbindWechatAccount(JeuciAccount jeuciAccount, out string urlOrMsg);
    }
}