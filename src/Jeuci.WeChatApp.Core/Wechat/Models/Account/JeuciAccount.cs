namespace Jeuci.WeChatApp.Wechat.Models.Account
{
    public class JeuciAccount
    {

        public JeuciAccount(WechatAccount wechatAccount)
        {
            UserWechatInfo = wechatAccount;

        }

        public bool IsBindWechat { get; }

        public bool IsBindEmail { get; }

        public string OpenId {
            get { return UserWechatInfo.OpenId; }
        }

        public string BindWechatAddress
        {
            get
            {
                return "/wechat/account/#/bindwechat";
            }
        }

        public UserInfo UserInfo { get; set; }

        public WechatAccount UserWechatInfo { get; }
    }
}
