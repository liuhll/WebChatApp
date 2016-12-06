using System.Threading.Tasks;
using Abp.Dependency;

namespace Jeuci.WeChatApp.Services.Email
{
    public interface IDirectEmailService : ITransientDependency
    {
        Task<bool> SendValidCodeByEmial(string emailAddresss,string body);
    }
}