using System.Threading.Tasks;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Wechat.Authentication;
using Jeuci.WeChatApp.Wechat.Models.Account;
using Senparc.Weixin.MP.CommonAPIs;

namespace Jeuci.WeChatApp.Wechat.Accounts.Impl
{
    public class BindAccount : IBindAccount
    {
        private readonly IWechatAuthentManager _wechatAuthentManager;

        public BindAccount(IWechatAuthentManager wechatAuthentManager)
        {
            _wechatAuthentManager = wechatAuthentManager;
        }

        public async Task<ResultMessage<bool>> BindWechatAccount(JeuciAccount input)
        {
            //1.判断用户输入的账号是否存在
            //2.微信号是否已经被绑定

            var accessToken = _wechatAuthentManager.GetAccessToken();
            await  CommonApi.GetUserInfoAsync(accessToken, input.OpenId);
             
            return new ResultMessage<bool>(true);


        }
    }
}