using Abp.AutoMapper;
using Jeuci.WeChatApp.Common.Enums;

namespace Jeuci.WeChatApp.WechatAccount.Dtos
{
    [AutoMap(typeof(Wechat.Models.Account.WechatAccount))]
    public class WechatAccountOutput
    {
        public string Subscribe { get; set; }

        public string OpenId { get; set; }

        public string NickName { get; set; }

        public Sex Sex { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Province { get; set; }

        public string Language { get; set; }

        public string HeadImgUrl { get; set; }

        public int Subscribe_Time { get; set; }

        public string Unionid { get; set; }

        public string Remark { get; set; }

        public string GroupId { get; set; }
    }
}
