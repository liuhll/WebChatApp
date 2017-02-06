using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeuci.WeChatApp.Wechat.Message;
using Senparc.Weixin.MP.Entities;

namespace Jeuci.WeChatApp.WechatMsg
{
    public class WechatMsgAppService : IWechatMsgAppService
    {
        private readonly IWechatMessageHandler _wechatMessageHandler;

        public WechatMsgAppService(IWechatMessageHandler wechatMessageHandler)
        {
            _wechatMessageHandler = wechatMessageHandler;
        }

        public string MsgHandlerByRequestContent(IRequestMessageBase requestMessage)
        {
            var responseMsg = _wechatMessageHandler.MsgHandlerByRequestContent(requestMessage);

            var responseDoc = Senparc.Weixin.MP.Helpers.EntityHelper.ConvertEntityToXml(responseMsg);

            return responseDoc.ToString();
        }
    }
}
