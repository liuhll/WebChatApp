using System.Threading.Tasks;
using Abp.Dependency;

namespace Jeuci.WeChatApp.Services.Email
{
    public interface IDirectEmailService : ITransientDependency
    {
        Task<bool> SendValidCodeByEmail(string emailAddresss,string body);
    }
}