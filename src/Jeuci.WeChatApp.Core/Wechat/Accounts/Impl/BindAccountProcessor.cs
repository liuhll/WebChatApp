using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Wechat.Authentication;
using Jeuci.WeChatApp.Wechat.Models.Account;
using Senparc.Weixin.MP.CommonAPIs;

namespace Jeuci.WeChatApp.Wechat.Accounts.Impl
{
    public class BindAccountProcessor : IBindAccountProcessor
    {
        private readonly IWechatAuthentManager  _wechatAuthentManager;
        private readonly IRepository<UserInfo> _userRepository;

        public BindAccountProcessor(IWechatAuthentManager wechatAuthentManager, 
            IRepository<UserInfo> userRepository)
        {
            _wechatAuthentManager = wechatAuthentManager;
            _userRepository = userRepository;
        }

        public ResultMessage<string> BindWechatAccount(JeuciAccount input)
        {
            //1.判断用户输入的账号是否存在
            //2.微信号是否已经被绑定

            input.SynchronWechatUserInfo(_wechatAuthentManager);

            input.SynchronUserInfo(_userRepository);
                    
            return new ResultMessage<string>("");

        }
    }
}