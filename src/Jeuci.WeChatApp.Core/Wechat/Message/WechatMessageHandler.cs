using System;
using System.Linq;
using System.Text;
using Abp.Domain.Repositories;
using Abp.Json;
using Abp.Logging;
using Jeuci.WeChatApp.Wechat.Models.Message;
using Senparc.Weixin.MP.Entities;
using Jeuci.WeChatApp.Common.Extensions;
using Jeuci.WeChatApp.Lottery.Server;
using Senparc.Weixin.MP;

namespace Jeuci.WeChatApp.Wechat.Message
{
    public class WechatMessageHandler : IWechatMessageHandler
    {
        private readonly IRepository<WechatMsg> _wechatMsgRepository;
        private readonly ILotteryServer lotteryServer;

        public WechatMessageHandler(IRepository<WechatMsg> wechatMsgRepository,
            ILotteryServer lotteryServer)
        {
            _wechatMsgRepository = wechatMsgRepository;
            this.lotteryServer = lotteryServer;
        }

        /// <summary>
        /// 根据请求内容处理微信消息
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ResponseMessageText MsgHandlerByRequestContent(IRequestMessageBase requestMessage)
        {
            ResponseMessageText responseMessage = null;
            switch (requestMessage.MsgType)
            {
                case RequestMsgType.Text://文字类型
                    var requestMsgText = requestMessage as RequestMessageText;
                    responseMessage = WechatTextMsgHandler(requestMsgText);

                    break;
                default:
                    throw new Exception("暂不支持其他类型的消息处理");
            }
            return responseMessage;


        }

        private ResponseMessageText WechatTextMsgHandler(RequestMessageText requestMessage)
        {
            ResponseMessageText strongresponseMessage = null;

            var msgInfo = _wechatMsgRepository.GetAll().FirstOrDefault(p => p.KeyWord.Contains(requestMessage.Content));
            if (msgInfo == null)
            {
                strongresponseMessage =
                    ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
                strongresponseMessage.Content = "不知道您在说什么";

                return strongresponseMessage;

            }
            strongresponseMessage =
                 ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            strongresponseMessage.Content = msgInfo.Sid == null ? msgInfo.ResponseMsg : GetLotteryPlanByCode(msgInfo);
            return strongresponseMessage;
        }

        private string GetLotteryPlanByCode(WechatMsg wechatMsg)
        {
            var respMsg = new StringBuilder(wechatMsg.ResponseMsg + ":\r\n");
            string msg = "";
            var planList = lotteryServer.GetPlanList((int)wechatMsg.Sid,out msg);

            var count = 1;
            LogHelper.Logger.Info("取到的计划:" + planList.Count);
            if (planList.Count>0)
            {
                foreach (var plan in planList)
                {
                    respMsg.Append(plan.PlanName + ":\r\n" + plan.PlanUrl + "\r\n");
                    count++;
                }
               
            }
            else
            {
                respMsg.Append("计划库中还没有存在该类型的计划");
            }
            return respMsg.ToString();
        }
    }
}