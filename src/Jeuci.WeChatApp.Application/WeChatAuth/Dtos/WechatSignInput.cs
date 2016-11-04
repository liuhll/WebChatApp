using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Wechat.Models;

namespace Jeuci.WeChatApp.WeChatAuth.Dtos
{
    [AutoMap(typeof(WechatSign))]
    public class WechatSignInput 
    {
        [Required]
        public string Signature { get; set; }

        [Required]
        public long Timestamp { get; set; }

        [Required]
        public int Nonce { get; set; }

        [Required]
        public string Echostr { get; set; }

    }
}