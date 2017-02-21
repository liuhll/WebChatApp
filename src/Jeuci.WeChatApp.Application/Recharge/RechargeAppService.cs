using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Logging;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Pay;
using Jeuci.WeChatApp.Pay.Lib;
using Jeuci.WeChatApp.Pay.Models;
using Jeuci.WeChatApp.Purchase.Dtos;

namespace Jeuci.WeChatApp.Recharge
{
    public class RechargeAppService : IRechargeAppService
    {
        private readonly IRechargeService _rechargeService;
        private readonly IPurchaseService _purchaseService;

        public RechargeAppService(IRechargeService rechargeService, IPurchaseService purchaseService)
        {
            _rechargeService = rechargeService;
            _purchaseService = purchaseService;
        }


        public ResultMessage<IList<int>> GetFreeList()
        {
            try
            {
                var freeList = _rechargeService.GetFreeList();
                return new ResultMessage<IList<int>>(freeList);
            }
            catch (Exception e)
            {
                return new ResultMessage<IList<int>>(ResultCode.Fail,e.Message);
            }
        }

        public ResultMessage<ServiceInfoOutput> GetUnifiedOrder(string openId,double fee)
        {
            var result = _purchaseService.UnifiedOrderResult(new ServiceOrder()
            {
                body = WxPayConfig.RECHARGE_NAME,
                openid = openId,
                total_fee = Convert.ToInt32(fee * 100),
            });

            var data = new ServiceInfoOutput()
            {
                OrderId = result.GetValue("orderid").ToString(),
                ServiceName = WxPayConfig.RECHARGE_NAME,
                OpenId = openId,
                OrderPrice = (decimal)fee,
                PrepayId = result.GetValue("prepay_id").ToString(),
                Description = "代理商在线充值服务",
                Sid = null,
            };
            return new ResultMessage<ServiceInfoOutput>(data);
        }

        public ResultMessage<string> CompleteRechargeOrder(WxPayData payData)
        {
            try
            {
                string msg = string.Empty;

                if (_rechargeService.CompleteRechargeOrder(payData, out msg))
                {
                    return new ResultMessage<string>(msg,"ok");
                }
                else
                {
                    LogHelper.Logger.Error("充值失败");
                    return new ResultMessage<string>(ResultCode.Fail, "充值失败", msg);
                }
               
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error(e.Message);
                return new ResultMessage<string>(ResultCode.Fail,"充值失败,异常",e.Message);
            }
        }

        public ResultMessage<string> ClientCompleteServiceOrder(PayOrderDto payOrder)
        {
            string msg = string.Empty;
            var result = _rechargeService.ClientCompleteServiceOrder(payOrder.MapTo<CompleteServiceOrder>(),out msg);

            try
            {
                if (result)
                {
                    return new ResultMessage<string>(msg,"OK");
                }
                return new ResultMessage<string>(ResultCode.Fail,"fail",msg);
            }
            catch (Exception e)
            {
                return new ResultMessage<string>(ResultCode.Fail, "fail", e.Message);
            }
        }
    }
}