using Abp.Dependency;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.Wechat.Password
{
    public interface IPassowrdProcessor : ITransientDependency
    {
        bool ModifyPassword(JeuciAccount jeuciAccount, string newPassworld, out string urlOrMsg);
    }
}