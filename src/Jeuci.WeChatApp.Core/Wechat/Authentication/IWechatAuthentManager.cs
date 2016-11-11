using Abp.Dependency;
using Jeuci.WeChatApp.Wechat.Models;

namespace Jeuci.WeChatApp.Wechat.Authentication
{
    public interface IWechatAuthentManager : ITransientDependency
    {
        bool CheckSignature(WechatSign wechatSign);

        string GetAccessToken();

    }
}