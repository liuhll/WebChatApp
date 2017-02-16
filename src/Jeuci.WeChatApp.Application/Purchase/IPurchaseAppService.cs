using System.Web.Http;
using Abp.Application.Services;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Pay.Lib;
using Jeuci.WeChatApp.Pay.Models;
using Jeuci.WeChatApp.Purchase.Dtos;

namespace Jeuci.WeChatApp.Purchase
{
    public interface IPurchaseAppService : IApplicationService
    {
        [HttpGet]
        ResultMessage<ServiceInfoOutput> GetUnifiedOrder(ServiceInfoInput input);

        WxpayOptionsOutput GetWxpayOptions(string nonceStr,string url);

        PayOptionsOutput GetPaySign(string package, string nonceStr);

        [HttpPost]
        ResultMessage<PayOrderDto> GeneratePayOrder(PayOrderInput payOrderInput);

        [HttpPut]
        ResultMessage<string> CompleteServiceOrder(WxPayData payData);

        void FailServiceOrder(UpdateServiceOrder order);

        ResultMessage<string> ClientCompleteServiceOrder(PayOrderDto payOrder);
    }
}