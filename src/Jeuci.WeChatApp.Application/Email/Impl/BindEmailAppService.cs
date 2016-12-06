using System.Threading.Tasks;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Email;
using Jeuci.WeChatApp.InfrastructureServices.DirectEmail;
using Jeuci.WeChatApp.InfrastructureServices.DirectEmail.Models;
using Jeuci.WeChatApp.Wechat.Accounts;

namespace Jeuci.WeChatApp.DirectEmail.Impl
{
    public class BindEmailAppService : IBindEmailAppService
    {
        private readonly IBindEmailProcessor _bindEmailProcessor;

        public BindEmailAppService(IBindEmailProcessor bindEmailProcessor)
        {
            _bindEmailProcessor = bindEmailProcessor;
        }

        public async Task<ResultMessage<string>> GetValidCodeByEmail(string openId, string emailAddress)
        {
            var result =await _bindEmailProcessor.SendValidByEmail(openId,emailAddress);
            if (result)
            {
                return new ResultMessage<string>("验证码发送成功");
            }
            return new ResultMessage<string>(ResultCode.Fail,"邮件发送失败，请稍后再尝试！");
        }
    }
}