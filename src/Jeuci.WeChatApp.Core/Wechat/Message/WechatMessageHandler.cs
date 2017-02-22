using System;
using System.Linq;
using System.Text;
using Abp.Domain.Repositories;
using Abp.Json;
using Abp.Logging;
using Jeuci.WeChatApp.Wechat.Models.Message;
using Senparc.Weixin.MP.Entities;
using Jeuci.WeChatApp.Common.Extensions;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Configs;
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
                //响应事件    
                case RequestMsgType.Event:
                    var request = requestMessage as RequestMessageEventBase;
                    responseMessage = WechatEventMsgHandler(request);
                    break;
                default:
                    LogHelper.Logger.Error("该消息并非文字类型消息,请求内容为:"+ requestMessage.ToJson());
                    throw new Exception("暂不支持其他类型的消息处理");
                  
            }
            return responseMessage;


        }

        private ResponseMessageText WechatEventMsgHandler(RequestMessageEventBase request)
        {
            ResponseMessageText responseMessage = null;
            switch (request.Event)
            {
                case Event.VIEW:
                  //  var viewRequest = request as RequestMessageEvent_View;
                    LogHelper.Logger.Debug("点击菜单跳转链接时的事件推送");
                    responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(request);
                    responseMessage.Content = "正在获取您的账号信息,请稍等...";
                    break;
                case Event.subscribe:
                    responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(request);
                    var msgInfo = _wechatMsgRepository.FirstOrDefault(p => p.KeyWord == "subscribe");
                    if (msgInfo !=null)
                    {
                        responseMessage.Content = msgInfo.ResponseMsg;
                    }
                    else
                    {
                        responseMessage.Content = "欢迎关注彩盟网";
                    }
                    break;
                case Event.CLICK:
                    var request_click = request as RequestMessageEvent_Click;
                    responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(request_click);
                    var respMsg = "欢迎关注彩盟网，我们为您提供了最准确，最齐全的彩票分析计划";
                    if (request_click.EventKey == WeChatConfig.MENU_SUB_CLICK_HELP)
                    {
                        var msgInfo1 = _wechatMsgRepository.FirstOrDefault(p => p.KeyWord == "help");
                        if (msgInfo1 != null)
                        {
                            respMsg = msgInfo1.ResponseMsg;
                        }
                    }
                    responseMessage.Content = respMsg;
                    break;
                default:
                    responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(request);
                    responseMessage.Content = _wechatMsgRepository.FirstOrDefault(p => p.KeyWord == "default").ResponseMsg;
                    break;

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
                strongresponseMessage.Content = _wechatMsgRepository.FirstOrDefault(p=>p.KeyWord == "default").ResponseMsg;

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