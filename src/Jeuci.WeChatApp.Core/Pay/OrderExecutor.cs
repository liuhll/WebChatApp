using System;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Logging;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Pay.Models;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.Pay
{
    public class OrderExecutor : IOrderExecutor
    {
        private readonly IRepository<UserPayOrderInfo, string> _userPayOrderRepository;
        private readonly IPurchaseService _purchaseService;
        private readonly IOrderPolicy _orderPolicy;

        public OrderExecutor(IRepository<UserPayOrderInfo, string> userPayOrderRepository, IPurchaseService purchaseService, IOrderPolicy orderPolicy)
        {
            _userPayOrderRepository = userPayOrderRepository;
            _purchaseService = purchaseService;
            _orderPolicy = orderPolicy;
        }

        public bool UpdateServiceOrder()
        {
            //查询超过20分钟状态小于等于0的订单
            var time = DateTime.Now.AddMinutes(-20);
            var needQueryOrderList = _userPayOrderRepository.GetAllList(p => p.PayAppID == WxPayConfig.APPID && p.State <= 0 && p.CreateTime < time);
            LogHelper.Logger.Debug("查询到未支付的订单有:"+needQueryOrderList.Count);

            int count1 = 0, count2 =0;
            foreach (var order in needQueryOrderList)
            {
                var orderId = WxPayConfig.MCHID + order.Id.Trim();
                var payData = _orderPolicy.Orderquery(orderId, OrderType.OutTradeNo);
                if (payData.GetValue("return_code").ToString() != "SUCCESS" ||
                    payData.GetValue("result_code").ToString() != "SUCCESS")
                {
                    //订单查询失败，
                    _purchaseService.FailServiceOrder(new UpdateServiceOrder()
                    {
                        ID = order.Id,
                        PayState = payData.GetValue("return_code").ToString(),
                        PayExtendInfo = "未查询到订单,直接关闭该订单",
                        State = 3,
                        
                    });
                    count1++;
                }
                else
                {
                    _purchaseService.CompleteServiceOrder(payData);
                    count2++;
                }

            }
            LogHelper.Logger.Debug(string.Format("未查询到的订单有:{0},已查询到的订单有{1}",count1,count2));
            return true;
        }
    }
}