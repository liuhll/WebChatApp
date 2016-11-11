using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Wechat.Menu;

namespace Jeuci.WeChatApp.WechatMenu
{
    public class WechatMenuAppService : IWechatMenuAppService
    {
        private readonly IWechatMenuManager _wechatMenuManager;

        public WechatMenuAppService(IWechatMenuManager wechatMenuManager)
        {
            _wechatMenuManager = wechatMenuManager;
        }

        public ResultMessage<bool> CreateWechatMenu()
        {
            string msg = string.Empty;
            return new ResultMessage<bool>(_wechatMenuManager.CreateWechatMenu(out msg), msg);
        }
    }
}