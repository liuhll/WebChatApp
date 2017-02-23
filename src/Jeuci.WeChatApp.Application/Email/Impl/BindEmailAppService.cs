using System;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Email;
using Jeuci.WeChatApp.Email.Dtos;
using Jeuci.WeChatApp.Wechat.Accounts;
using Jeuci.WeChatApp.Wechat.Models.BindEmail;

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
            try
            {
                var result =await _bindEmailProcessor.SendValidByEmail(openId,emailAddress);
                if (result)
                {
                    return new ResultMessage<string>("验证码已发送，如果没有收到，请检查垃圾箱");
                }
                return new ResultMessage<string>(ResultCode.Fail,"邮件发送失败，请稍后再尝试！");
            }
            catch (Exception e)
            {
                return new ResultMessage<string>(ResultCode.ServiceError,e.Message);
            }
        }

        public ResultMessage<string> BindUserEmail(BindEmailInput input)
        {
            string msgOrUrl;
            var result =  _bindEmailProcessor.BindUserEmail(input.MapTo<BindEmailModel>(),out msgOrUrl);
            try
            {
                if (result)
                {
                    return new ResultMessage<string>(msgOrUrl,"您的电子邮箱绑定成功！");
                }
                return new ResultMessage<string>(ResultCode.Fail,msgOrUrl);
            }
            catch (Exception e)
            {
                return new ResultMessage<string>(ResultCode.ServiceError,e.Message);
            }
        }
    }
}