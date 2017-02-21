using System;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Logging;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Pay;
using Jeuci.WeChatApp.Pay.Lib;
using Jeuci.WeChatApp.Pay.Models;
using Jeuci.WeChatApp.Pay.Tool;
using Jeuci.WeChatApp.Purchase.Dtos;
using Jeuci.WeChatApp.Wechat.Authentication;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.Purchase
{
    public class PurchaseAppService : IPurchaseAppService
    {
        private readonly  IPurchaseService _purchaseService;
        private readonly IRepository<UserInfo> _userRepository;
        private readonly IWechatAuthentManager _wechatAuthentManager;

        public PurchaseAppService(IPurchaseService purchaseService, 
            IRepository<UserInfo> userRepository, IWechatAuthentManager wechatAuthentManager)
        {
            _purchaseService = purchaseService;
            _userRepository = userRepository;
            _wechatAuthentManager = wechatAuthentManager;
        }

        public ResultMessage<ServiceInfoOutput> GetUnifiedOrder(ServiceInfoInput input)
        {
            try
            {
                var result =  _purchaseService.UnifiedOrderResult(new ServiceOrder()
                {
                    body = input.body,
                    openid = input.openid,
                    total_fee =  Convert.ToInt32(input.total_fee * 100),
                });
                var data = new ServiceInfoOutput()
                {
                    OrderId = result.GetValue("orderid").ToString(),
                    ServiceName = input.body,
                    OpenId = input.openid,
                    OrderPrice = input.total_fee,
                    PrepayId = result.GetValue("prepay_id").ToString(),
                    Description = input.description,
                    Sid = input.sid,
                };
                return new ResultMessage<ServiceInfoOutput>(data);
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("订单生成失败！" + e.Message);
                return new ResultMessage<ServiceInfoOutput>(ResultCode.Fail,"订单生产失败,原因:"+e.Message+",请重试！");
            }
            
        }

        public WxpayOptionsOutput GetWxpayOptions(string nonceStr,string url)
        {
            var result = _purchaseService.GetWxpayOptions(nonceStr,url);
            return result.MapTo<WxpayOptionsOutput>();
        }

        public PayOptionsOutput GetPaySign(string package, string nonceStr)
        {
            var result = _purchaseService.GetPaySign(package,nonceStr);
            return result.MapTo<PayOptionsOutput>();
        }

        public ResultMessage<PayOrderDto> GeneratePayOrder(PayOrderInput payOrderInput)
        {
            try
            {
                var userInfo = _userRepository.FirstOrDefault(p => p.WeChat == payOrderInput.OpenId);
                if (userInfo == null)
                {
                    LogHelper.Logger.Error(string.Format("获取用户失败，不存在该OpenId：{0}对应的用户",payOrderInput.OpenId));
                    return new ResultMessage<PayOrderDto>(ResultCode.Fail, "生成订单失败，请确保您已经绑定了彩盟网账号.");
                }

                var payOrder = new PayOrder()
                {
                    ID = payOrderInput.ID.Substring(10),
                    UId = userInfo.Id,
                    Cost = payOrderInput.Cost,
                  //  GoodsInfo = payOrderInput.GoodsInfo,
                    GoodsType = payOrderInput.GoodType,
                    PayAppID = _wechatAuthentManager.AppId,
                    PayMode = 2,
                    PayType = 1,
                    GoodsName = payOrderInput.GoodsName,
                    GoodsID = payOrderInput.GoodsId,
                    Remarks = "微信公众号支付",

                };
                string msg = string.Empty;
                var result = _purchaseService.GeneratePayOrder(payOrder,out msg);
                if (result)
                {
                    return new ResultMessage<PayOrderDto>(ResultCode.Success, msg, new PayOrderDto()
                    {
                        OrderID = payOrder.ID,
                        Cost = payOrder.Cost,
                        NewID = OrderHelper.GenerateNewId(),
                        Remarks = payOrder.Remarks,
                    });
                }
                return new ResultMessage<PayOrderDto>(ResultCode.Fail, msg);
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error(e.Message,e);
                return new ResultMessage<PayOrderDto>(e);
            }
        }

        public ResultMessage<string> CompleteServiceOrder(WxPayData payData)
        {
            var result = _purchaseService.CompleteServiceOrder(payData);
            string msg = string.Empty;
            if (result == 0)
            {
                msg = "服务购买成功！您可以重新登录App后,享受我们提供的服务!";
                return new ResultMessage<string>(msg);
            }
            switch (result)
            {
                case -1:
                    msg = "异常失败";
                    break;
                case -2:
                    msg = "订单不存在";
                    break;
                case -3:
                    msg = "金额不一致";
                    break;
                case 1:
                    msg = "订单之前已处理过，无需重复处理 ";
                    break;
                case 2:
                    msg = "客户已有更高授权";
                    break;
                case 3:
                    msg = "客户已有此版本的终身授权";
                    break;
                default:
                    msg = "未知原因，购买失败！";
                    break;

            }
            return new ResultMessage<string>(ResultCode.Fail,"fail",msg);
           
        }

        public void FailServiceOrder(UpdateServiceOrder order)
        {
            _purchaseService.FailServiceOrder(order);
        }

        public ResultMessage<string> ClientCompleteServiceOrder(PayOrderDto payOrder)
        {
            var result = _purchaseService.ClientCompleteServiceOrder(payOrder.MapTo<CompleteServiceOrder>());

            string msg = string.Empty;
            if (result == 0)
            {
                msg = "服务购买成功！您可以重新登录App后,享受我们提供的服务!";
                return new ResultMessage<string>(msg);
            }
            switch (result)
            {
                case -1:
                    msg = "异常失败";
                    break;
                case -2:
                    msg = "订单不存在";
                    break;
                case -3:
                    msg = "金额不一致";
                    break;
                case 1:
                    msg = "订单之前已处理过，无需重复处理 ";
                    break;
                case 2:
                    msg = "客户已有更高授权";
                    break;
                case 3:
                    msg = "客户已有此版本的终身授权";
                    break;
                default:
                    msg = "未知原因，购买失败！";
                    break;

            }
            return new ResultMessage<string>(ResultCode.Fail, "FAIL", msg);
        }

     
    }
}