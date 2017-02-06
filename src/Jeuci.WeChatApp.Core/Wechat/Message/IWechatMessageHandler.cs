using Abp.Dependency;
using Senparc.Weixin.MP.Entities;

namespace Jeuci.WeChatApp.Wechat.Message
{
    public interface IWechatMessageHandler : ITransientDependency
    {
        ResponseMessageText MsgHandlerByRequestContent(IRequestMessageBase requestMessage);
    }
}