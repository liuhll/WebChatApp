using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.WechatAccount.Dtos
{
    [AutoMap(typeof(JeuciAccount))]
    public class BindAccountInput 
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_]{3,16}$",ErrorMessage = "用户名非法")]
        public string Account { get; set; }
        
        [Required]
        public string Password { get; set; }

        [Required]
        public string OpenId { get; set; }

    }
}