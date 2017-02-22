using System;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Logging;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Wechat.Accounts;
using Jeuci.WeChatApp.Wechat.Authentication;
using Jeuci.WeChatApp.Wechat.Models.Account;
using Jeuci.WeChatApp.Wechat.Password;
using Jeuci.WeChatApp.WechatAccount.Dtos;

namespace Jeuci.WeChatApp.WechatAccount
{
    public class WechatAccountAppService : IWechatAccountAppService
    {
        private readonly IWechatOAuth2Processor _wechatOAuth2Processor;
        private readonly IBindAccountProcessor _bindAccountProcessor;
        private readonly IPassowrdProcessor _passowrdProcesssor;

        public WechatAccountAppService(IWechatOAuth2Processor wechatOAuth2Processor,
            IBindAccountProcessor bindAccountProcessor, 
            IPassowrdProcessor passowrdProcesssor)
        {
            _wechatOAuth2Processor = wechatOAuth2Processor;
            _bindAccountProcessor = bindAccountProcessor;
            _passowrdProcesssor = passowrdProcesssor;

        }

        public ResultMessage<JeuciAccountOutput> GetWechatUserInfo(string openId)
        {
            try
            {
                var jeuciAccount = _wechatOAuth2Processor.GetWechatUserInfo(openId);             
                return new ResultMessage<JeuciAccountOutput>(jeuciAccount.MapTo<JeuciAccountOutput>());
            }
            catch (Exception e)
            {
                return new ResultMessage<JeuciAccountOutput>(ResultCode.ServiceError,e.Message);
            }
        }

        public ResultMessage<string> BindWechatAccount(BindAccountInput input)
        {
            try
            {
                string urlOrMsg;
                var isSuccess  = _bindAccountProcessor.BindWechatAccount(new JeuciAccount(input.OpenId, input.Account, input.Password, AccountOperateType.BindAccount), out urlOrMsg);
                if (isSuccess)
                {
                    return new ResultMessage<string>(urlOrMsg,"绑定成功！");
                }
                return new ResultMessage<string>(ResultCode.Fail,urlOrMsg);
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error(e.Message);
                return new ResultMessage<string>(ResultCode.ServiceError,e.Message);
            }
        }

        public ResultMessage<string> UnbindWechatAccount(UnBindAccountInput input)
        {
            string urlOrMsg;
            try
            {
                var isSuccess = _bindAccountProcessor.UnbindWechatAccount(new JeuciAccount(input.OpenId, input.Account, input.Password,AccountOperateType.UnBindAccount),out urlOrMsg);
                if (isSuccess)
                {
                    return new ResultMessage<string>(urlOrMsg,"账号解绑成功，您可以绑定其他掌赢专家账号！");
                }
                return new ResultMessage<string>(ResultCode.Fail, urlOrMsg);
            }
            catch (Exception e)
            {
                return new ResultMessage<string>(ResultCode.Fail, e.Message);
            }
        }

        public ResultMessage<string> Password(ModifyPasswordInput input)
        {
            string urlOrMsg;
            try
            {
                var isSuccess = _passowrdProcesssor.ModifyPassword(new JeuciAccount(input.OpenId, input.AccountName, input.OldPassword, AccountOperateType.ModifyPassword),input.NewPassworld, out urlOrMsg);
                if (isSuccess)
                {
                    return new ResultMessage<string>(urlOrMsg, "密码修改成功，请使用新密码登录您的App！");
                }
                return new ResultMessage<string>(ResultCode.Fail, urlOrMsg);
            }
            catch (Exception e)
            {
                return new ResultMessage<string>(ResultCode.Fail, e.Message);
            }
        }

        public async Task<ResultMessage<string>> RetrievePwdValidCode(string openId, string email)
        {
            try
            {
                var result = await _passowrdProcesssor.SendRetrievePwdValidCode(openId, email);
                if (result)
                {
                    return new ResultMessage<string>("验证码发送成功,请在有效期内完成重置密码操作");
                }
                return new ResultMessage<string>(ResultCode.Fail, "邮件发送失败，请稍后再尝试！");
            }
            catch (Exception e)
            {
                return new ResultMessage<string>(ResultCode.ServiceError, e.Message);
            }
        }

        public ResultMessage<string> RetrievePwd(RetrievePwdInput input)
        {
            string urlOrMsg;
            var isSuccess = _passowrdProcesssor.RetrievePwd(new JeuciAccount(input.OpenId, AccountOperateType.RetrievePwd), input.NewPassword,input.ValidCode, out urlOrMsg);
            try
            {
                if (isSuccess)
                {
                    return new ResultMessage<string>(urlOrMsg, "密码重置成功，请使用新密码登录您的App！");
                }
                return new ResultMessage<string>(ResultCode.Fail, urlOrMsg);
            }
            catch (Exception e)
            {
                return new ResultMessage<string>(ResultCode.Fail, e.Message);
            }
        }
    }
}
