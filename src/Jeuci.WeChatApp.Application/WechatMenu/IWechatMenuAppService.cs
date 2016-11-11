using Abp.Application.Services;
using Jeuci.WeChatApp.Common;

namespace Jeuci.WeChatApp.WechatMenu
{
    public interface IWechatMenuAppService : IApplicationService
    {
        ResultMessage<bool> CreateWechatMenu();
    }
}