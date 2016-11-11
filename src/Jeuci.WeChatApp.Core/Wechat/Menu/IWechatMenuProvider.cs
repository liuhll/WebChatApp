using Abp.Dependency;
using Senparc.Weixin.MP.Entities.Menu;

namespace Jeuci.WeChatApp.Wechat.Menu
{
    public interface IWechatMenuProvider : ITransientDependency
    {

        ButtonGroup GetWechatMenu();

    }
}