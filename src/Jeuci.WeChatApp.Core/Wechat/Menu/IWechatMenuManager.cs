using System.Threading.Tasks;
using Abp.Dependency;

namespace Jeuci.WeChatApp.Wechat.Menu
{
    public interface IWechatMenuManager : ITransientDependency
    {
        bool CreateWechatMenu(out string msg);
    }
}