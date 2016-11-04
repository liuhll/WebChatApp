using Abp.Dependency;
using Jeuci.WeChatApp.WeChatAuth.Dtos;

namespace Jeuci.WeChatApp.WeChatAuth
{
    public interface IWechatAuthAppService : ITransientDependency
    {
        bool CheckSignature(WechatSignInput input);
    }
}