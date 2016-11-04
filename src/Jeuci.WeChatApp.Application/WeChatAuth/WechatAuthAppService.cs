using Abp.AutoMapper;
using Jeuci.WeChatApp.Wechat.Authentication;
using Jeuci.WeChatApp.Wechat.Models;
using Jeuci.WeChatApp.WeChatAuth.Dtos;

namespace Jeuci.WeChatApp.WeChatAuth
{
    public class WechatAuthAppService : IWechatAuthAppService
    {
        private readonly IWechatAuthentManager _wechatAuthentManager;

        public WechatAuthAppService(IWechatAuthentManager wechatAuthentManager)
        {
            _wechatAuthentManager = wechatAuthentManager;
        }

        public bool CheckSignature(WechatSignInput input)
        {
            return _wechatAuthentManager.CheckSignature(input.MapTo<WechatSign>());
        }
    }
}