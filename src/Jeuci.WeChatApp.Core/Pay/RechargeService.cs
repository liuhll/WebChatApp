using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Logging;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Pay.Lib;
using Jeuci.WeChatApp.Pay.Models;
using Jeuci.WeChatApp.Pay.Tool;
using Jeuci.WeChatApp.Repositories;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.Pay
{
    public class RechargeService : IRechargeService
    {
        private readonly IRepository<UserPayOrderInfo, string> _userPayOrdeRepository;

        private readonly IPurchaseServiceRepository _purchaseServiceRepository;

        private readonly IRepository<UserRecharge, string> _userRechargeRepository;

        private readonly IRepository<UserInfo> _userRepository;

        private readonly IOrderPolicy _orderPolicy;

        public RechargeService(IRepository<UserPayOrderInfo, string> userPayOrdeRepository,
            IPurchaseServiceRepository purchaseServiceRepository, IRepository<UserRecharge, string> userRechargeRepository, IRepository<UserInfo> userRepository, IOrderPolicy orderPolicy)
        {
            _userPayOrdeRepository = userPayOrdeRepository;
            _purchaseServiceRepository = purchaseServiceRepository;
            _userRechargeRepository = userRechargeRepository;
            _userRepository = userRepository;
            _orderPolicy = orderPolicy;
        }

        public IList<int> GetFreeList()
        {
            return ConfigHelper.GetValuesByKey("RechargeFreeList").Split(',').Select(p=>Convert.ToInt32(p)).ToList();
        }
        /**
         *状态 2 为支付成功， =3 位支付失败
         * 
        **/
        [UnitOfWork]
        public bool CompleteRechargeOrder(WxPayData queryData,out string msg)
        {
            string out_trade_no = queryData.GetValue("out_trade_no").ToString().Substring(10);
            //取出提交的数据包，原样返回
            //object attachData = payData.GetValue("attach");
            //交易状态
            string trade_state = queryData.GetValue("trade_state").ToString();
            //微信支付订单号
            string transaction_id = null;
            if (queryData.IsSet("transaction_id"))
                transaction_id = queryData.GetValue("transaction_id").ToString();

            var orderInfo = _userPayOrdeRepository.FirstOrDefault(p => p.Id == out_trade_no);
            orderInfo.UpdateTime = DateTime.Now;
            if (trade_state == "SUCCESS")//交易成功
            {
                try
                {
                    var userInfo = _userRepository.Get(orderInfo.UId);
                    var fee = Convert.ToDecimal(queryData.GetValue("total_fee"))/100;
                    //_purchaseServiceRepository.UpdateServiceOrder(new UpdateServiceOrder()
                    //{
                    //    ID = out_trade_no,
                    //    PayExtendInfo = queryData.ToJson(),
                    //    PayState = trade_state,
                    //    PayOrderID = transaction_id,
                    //    State = 2
                    //});

                    orderInfo.PayExtendInfo = queryData.ToJson();
                    orderInfo.PayState = trade_state;
                    orderInfo.PayOrderID = transaction_id;
                    orderInfo.State = 2;
                    _userPayOrdeRepository.Update(orderInfo);

                    _userRechargeRepository.Insert(new UserRecharge()
                    {
                        Id = OrderHelper.GenerateNewId(),
                        Cost = fee,
                        AdminId = null,
                        CreateTime = DateTime.Now,
                        OrderID = out_trade_no,
                        Remarks = "微信公众号充值",
                        UId = userInfo.Id
                    });
                    userInfo.Fund += fee;
                    _userRepository.UpdateAsync(userInfo);
                    msg = "充值成功";
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            else if (trade_state == "USERPAYING")//正在支付
            {

                //var result = _purchaseServiceRepository.UpdateServiceOrder(new UpdateServiceOrder()
                //{
                //    ID = out_trade_no,
                //    PayExtendInfo = queryData.ToJson(),
                //    PayState = trade_state,
                //    PayOrderID = transaction_id,
                //    State = null
                //});

                orderInfo.PayExtendInfo = queryData.ToJson();
                orderInfo.PayState = trade_state;
                orderInfo.PayOrderID = transaction_id;
                orderInfo.State = null;
                _userPayOrdeRepository.Update(orderInfo);

                msg = "充值失败, 订单正在支付中";
                return false;
            }
            else if (trade_state == "NOTPAY")
            {
                //算作超时关闭订单
                if (orderInfo != null && orderInfo.CreateTime.AddMinutes(20) < DateTime.Now)
                {
                    //var result1 = _purchaseServiceRepository.UpdateServiceOrder(new UpdateServiceOrder()
                    //{
                    //    ID = out_trade_no,
                    //    PayExtendInfo = queryData.ToJson(),
                    //    PayState = trade_state,
                    //    PayOrderID = transaction_id,
                    //    State = 3
                    //});

                    orderInfo.PayExtendInfo = queryData.ToJson();
                    orderInfo.PayState = trade_state;
                    orderInfo.PayOrderID = transaction_id;
                    orderInfo.State = 3;
                    _userPayOrdeRepository.Update(orderInfo);

                    LogHelper.Logger.Debug("超时关闭订单");
                    msg = "支付超时，订单被关闭";
                    return false;
                }
                else
                {
                    //继续等待，还不算结束
                    //var result1 = _purchaseServiceRepository.UpdateServiceOrder(new UpdateServiceOrder()
                    //{
                    //    ID = out_trade_no,
                    //    PayExtendInfo = queryData.ToJson(),
                    //    PayState = trade_state,
                    //    PayOrderID = transaction_id,
                    //    State = null
                    //});

                    orderInfo.PayExtendInfo = queryData.ToJson();
                    orderInfo.PayState = trade_state;
                    orderInfo.PayOrderID = transaction_id;
                    orderInfo.State = null;
                    _userPayOrdeRepository.Update(orderInfo);
                    msg = "订单尚未支付";
                    return false;

                }
            }
            else
            {
                //var result1 = _purchaseServiceRepository.UpdateServiceOrder(new UpdateServiceOrder()
                //{
                //    ID = out_trade_no,
                //    PayExtendInfo = queryData.ToJson(),
                //    PayState = trade_state,
                //    PayOrderID = transaction_id,
                //    State = 3
                //});

                orderInfo.PayExtendInfo = queryData.ToJson();
                orderInfo.PayState = trade_state;
                orderInfo.PayOrderID = transaction_id;
                orderInfo.State = 3;
                orderInfo.UpdateTime = DateTime.Now;
                _userPayOrdeRepository.Update(orderInfo);
                msg = "支付失败";
                return false;
            }
        }

        public bool ClientCompleteServiceOrder(CompleteServiceOrder order, out string msg)
        {
            LogHelper.Logger.Debug("订单信息:" + order.ToJson());
            var orderId = order.OrderID;
            var orderInfo = _userPayOrdeRepository.FirstOrDefault(p => p.Id == orderId);
            if (orderInfo == null)
            {
                msg = "没有查询到相关的订单";
                return false;
            }

            LogHelper.Logger.Debug("查询到的订单信息为：" + orderInfo.ToJson());

            if (orderInfo.State == 2)
            {
                msg = "充值成功，您可以登陆代理系统查看您的充值金额";
                return true;
            }


            var query = _orderPolicy.Orderquery(string.Format("{0}{1}", WxPayConfig.MCHID, order.OrderID), OrderType.OutTradeNo);

            if (query == null)
            {
                LogHelper.Logger.Error("没有查询到相关的订单信息");

                orderInfo.State = 3;
                orderInfo.PayState = "FAIL";
                orderInfo.UpdateTime = DateTime.Now;
                _userPayOrdeRepository.Update(orderInfo);
                msg = "没有查询到相关的订单信息";
                return false;
            }
            else
            {
                LogHelper.Logger.Debug(query.ToJson());
                if (query.IsSet("trade_state") && query.GetValue("trade_state").ToString() == "SUCCESS")
                {
                //    var orderState = _purchaseServiceRepository.CompleteServiceOrder(new CompleteServiceOrder()
                //    {
                //        OrderID = orderId,
                //        Cost = (double)Convert.ToInt32(query.GetValue("total_fee")) / 100,
                //        NewID = OrderHelper.GenerateNewId(),
                //        PayState = query.GetValue("trade_state").ToString(),
                //        PayExtendInfo = query.ToXml(),
                //        PayOrderID = query.GetValue("transaction_id").ToString(),
                //        Remarks = "微信公众号支付"
                //    });
                    var orderState = CompleteRechargeOrder(query, out msg);

                    return orderState;
                }
                else
                {
                    msg = "支付失败";
                    return false;
                }
            }
        }
    }
}