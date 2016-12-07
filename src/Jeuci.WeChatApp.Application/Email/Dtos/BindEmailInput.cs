using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Jeuci.WeChatApp.Wechat.Models.BindEmail;

namespace Jeuci.WeChatApp.Email.Dtos
{
    [AutoMap(typeof(BindEmailModel))]
    public class BindEmailInput
    {
        [Required]
        public string OpenId { get; set; }

        [Required]
        public string AccountName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }

        [Required]        
        public string ValidCode { get; set; }
    }
}