using System.Threading.Tasks;
using Abp.Dependency;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.Wechat.Password
{
    public interface IPassowrdProcessor : ITransientDependency
    {
        bool ModifyPassword(JeuciAccount jeuciAccount, string newPassworld, out string urlOrMsg);

        Task<bool> SendRetrievePwdValidCode(string openId, string email);

        bool RetrievePwd(JeuciAccount jeuciAccount, string newPassword,string validCodeStr, out string urlOrMsg);
    }
}