using System;
using Abp.Domain.Repositories;
using Abp.Logging;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Pay.Models;


namespace Jeuci.WeChatApp.Pay
{
    public class OrderExecutor : IOrderExecutor
    {
       
        private readonly IPurchaseService _purchaseService;
        private readonly IOrderPolicy _orderPolicy;
        private readonly IRechargeService _rechargeService;
        private readonly IRepository<UserPayOrderInfo, string> _userpayOrderRepository;

        public OrderExecutor(IPurchaseService purchaseService, IOrderPolicy orderPolicy,
            IRechargeService rechargeService, 
            IRepository<UserPayOrderInfo, string> userpayOrderRepository)
        {
            _purchaseService = purchaseService;
            _orderPolicy = orderPolicy;
            _rechargeService = rechargeService;
            _userpayOrderRepository = userpayOrderRepository;
        }

        public bool UpdateServiceOrder()
        {
            //查询超过20分钟状态小于等于0的订单
            var needQueryOrderList = _purchaseService.GetNeedQueryOrderList(PayMode.MobileWeb);
            LogHelper.Logger.Debug("查询到未支付的订单有:"+ needQueryOrderList.Count);

            if (needQueryOrderList.Count <= 0)
            {
                LogHelper.Logger.Debug("当前没有未关闭的订单");
                return true;
            }

            int count1 = 0, count2 =0;
            foreach (var order in needQueryOrderList)
            {
                var orderId = WxPayConfig.MCHID + order.Id.Trim();
                var payData = _orderPolicy.Orderquery(orderId, OrderType.OutTradeNo);
                if (payData.GetValue("return_code").ToString() != "SUCCESS" ||
                    payData.GetValue("result_code").ToString() != "SUCCESS")
                {
                    //订单查询失败，
                    order.UpdateTime = DateTime.Now;
                    order.PayExtendInfo = "未查询到订单的支付信息,直接关闭该订单";
                    order.State = 3;
                    order.PayState = payData.GetValue("return_code").ToString();
                    //_purchaseService.FailServiceOrder(new UpdateServiceOrder()
                    //{
                    //    ID = order.Id,
                    //    PayState = payData.GetValue("return_code").ToString(),
                    //    PayExtendInfo = "未查询到订单,直接关闭该订单",
                    //    State = 3,
                        
                    //});
                    _userpayOrderRepository.Update(order);
                    count1++;
                }
                else
                {
                    if (order.GoodsType == 0)
                    {
                        string msg = string.Empty;
                        _rechargeService.CompleteRechargeOrder(payData,out msg);
                        LogHelper.Logger.Info(msg);
                    }
                    else
                    {
                        _purchaseService.CompleteServiceOrder(payData);
                    }
                   
                    count2++;
                }

            }
            LogHelper.Logger.Debug(string.Format("未查询到的订单有:{0},查询到并处理的订单有{1}",count1,count2));
            return true;
        }
    }
}