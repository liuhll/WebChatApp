using Abp.Application.Services;
using Senparc.Weixin.MP.Entities;

namespace Jeuci.WeChatApp.WechatMsg
{
    public interface IWechatMsgAppService : IApplicationService
    {
        string MsgHandlerByRequestContent(IRequestMessageBase requestMessage);
    }
}