namespace Jeuci.WeChatApp.WechatAccount.Dtos
{
    public class RetrievePwdInput
    {
        public string OpenId { get; set; }

        public string NewPassword { get; set; }

        public string ValidCode { get; set; }
    }
}