namespace Jeuci.WeChatApp.Wechat.Models.Account
{
    public class JeuciAccount
    {
        public string Account { get; set; }

        public string Password { get; set; }

        public string OpenId { get; set; }

        public WechatAccount UserWechatInfo { get; set; }
    }
}
