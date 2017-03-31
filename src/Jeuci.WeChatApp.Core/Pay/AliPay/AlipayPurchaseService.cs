using System;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Logging;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Pay.Lib;
using Jeuci.WeChatApp.Pay.Models;
using Jeuci.WeChatApp.Repositories;
using Jeuci.WeChatApp.Pay.Tool;

namespace Jeuci.WeChatApp.Pay.AliPay
{
    public class AlipayPurchaseService : IAlipayPurchaseService
    {
        private readonly IAlipayRequest _alipayRequest;

        private readonly IPurchaseService _purchaseService;

        private readonly IRepository<UserPayOrderInfo,string> _uerPayOrderInfoRepository;

        private readonly IPurchaseServiceRepository _purchaseServiceRepository;

        public AlipayPurchaseService(IAlipayRequest alipayRequest, IPurchaseService purchaseService, IRepository<UserPayOrderInfo, string> uerPayOrderInfoRepository, IPurchaseServiceRepository purchaseServiceRepository)
        {
            _alipayRequest = alipayRequest;
            _purchaseService = purchaseService;
            _uerPayOrderInfoRepository = uerPayOrderInfoRepository;
            _purchaseServiceRepository = purchaseServiceRepository;
        }

        [UnitOfWork]
        public bool PayOrder(AliPayOrder input, out string msg)
        {
            try
            {
                var payOrder = new PayOrder()
                {
                    ID = input.ID,
                    Cost = input.Cost,
                    GoodsID = input.GoodsId,
                    GoodsName = input.GoodsName,
                    GoodsType = input.GoodType,
                    PayAppID = AliPayConfig.APPID,
                    PayType = Convert.ToInt32(PayType.AliPay),
                    PayMode = Convert.ToInt32(PayMode.MobileWeb),
                    UId = input.Uid,
                    Remarks = "支付宝手机网页支付",

                };

                var isNewPayOrderSuccess = _purchaseService.GeneratePayOrder(payOrder, out msg);
                if (!isNewPayOrderSuccess)
                {
                    return false;
                }

                var options = new AlipayOrderOptions()
                {
                    out_trade_no = input.ID,
                    seller_id = AliPayConfig.PID,
                    subject = input.GoodsName,
                    total_amount = input.Cost.ToString("0.00")
                };
                _alipayRequest.Wappay(options,out msg);  
                return true;
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error(e.Message);
                msg = e.Message;
                return false;
            }
           
        }

        public bool AlipayCallBack(AlipayData alipayData)
        {
            var userPayInfo = _uerPayOrderInfoRepository.Get(alipayData.GetValue("out_trade_no"));
            if (userPayInfo == null)
            {
                LogHelper.Logger.Error(string.Format("数据库中不存在单号为{0}的订单", alipayData.GetValue("out_trade_no")));
                return false;
            }
            if (!VerifyOrder(alipayData,userPayInfo))
            {
                return false;
            }
            var tradeStatus = alipayData.GetValue("trade_status");
            var transactionId = string.Empty;
            if (alipayData.IsSet("trade_no"))
            {
                transactionId = alipayData.GetValue("trade_no");
            }
            if (tradeStatus.Equals("TRADE_SUCCESS") || tradeStatus.Equals("TRADE_FINISHED"))
            {
                //支付金额
                return _purchaseServiceRepository.CompleteServiceOrder(new CompleteServiceOrder()
                {
                    OrderID = alipayData.GetValue("out_trade_no"),
                    Cost = Convert.ToDouble(alipayData.GetValue("invoice_amount")),
                    NewID = OrderHelper.GenerateNewId(),
                    PayExtendInfo = alipayData.ToJson(),
                    PayState = tradeStatus,
                    PayOrderID = transactionId,
                    Remarks = "支付宝手机网页支付",

                }) == 0;
            }
            //超时关闭
            else if (tradeStatus.Equals("TRADE_CLOSED"))
            {
                LogHelper.Logger.Debug("订单超时关闭");
                return _purchaseServiceRepository.UpdateServiceOrder(new UpdateServiceOrder()
                {
                    ID = alipayData.GetValue("out_trade_no"),
                    PayExtendInfo = alipayData.ToJson(),
                    PayState = tradeStatus,
                    PayOrderID = transactionId,
                    State = 3,
                }) == 0;
            }
            else
            {
                return false;
            }
        }

        public void UpdateAliPayOrder(AlipayData alipayData)
        {
            var userPayInfo = _uerPayOrderInfoRepository.Get(alipayData.GetValue("out_trade_no"));
            if (userPayInfo == null)
            {
                LogHelper.Logger.Error(string.Format("数据库中不存在单号为{0}的订单", alipayData.GetValue("out_trade_no")));              
            }
            var tradeStatus = alipayData.GetValue("trade_status");
            var transactionId = string.Empty;
            if (alipayData.IsSet("trade_no"))
            {
                transactionId = alipayData.GetValue("trade_no");
            }
            if (tradeStatus.Equals("TRADE_SUCCESS") || tradeStatus.Equals("TRADE_FINISHED"))
            {
                //支付金额
                 _purchaseServiceRepository.CompleteServiceOrder(new CompleteServiceOrder()
                {
                    OrderID = alipayData.GetValue("out_trade_no"),
                    Cost = Convert.ToDouble(alipayData.GetValue("invoice_amount")),
                    NewID = OrderHelper.GenerateNewId(),
                    PayExtendInfo = alipayData.ToJson(),
                    PayState = tradeStatus,
                    PayOrderID = transactionId,
                    Remarks = "支付宝手机网页支付",
                });
            }
            //超时关闭
            else
            {
                LogHelper.Logger.Debug("订单超时关闭");
                _purchaseServiceRepository.UpdateServiceOrder(new UpdateServiceOrder()
                {
                    ID = alipayData.GetValue("out_trade_no"),
                    PayExtendInfo = alipayData.ToJson(),
                    PayState = tradeStatus,
                    PayOrderID = transactionId,
                    State = 3,
                });
            }
        }

        private bool VerifyOrder(AlipayData alipayData, UserPayOrderInfo payOrder)
        {
            if (Convert.ToDecimal(alipayData.GetValue("invoice_amount")) != payOrder.Cost)
            {
                LogHelper.Logger.Error("用户支付的金额与订单预付款金额不一致,交易失败");
                return false;
            }
            if (!AliPayConfig.PID.Equals(alipayData.GetValue("seller_id")))
            {
                LogHelper.Logger.Error("seller_id不一致，交易失败");
                return false;
            }
            if (!AliPayConfig.APPID.Equals(alipayData.GetValue("app_id")))
            {
                LogHelper.Logger.Error("app_id不一致，交易失败");
                return false;
            }
            return true;

        }
    }
}