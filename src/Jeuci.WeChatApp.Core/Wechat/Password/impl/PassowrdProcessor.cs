using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Logging;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Services.Email;
using Jeuci.WeChatApp.Wechat.Authentication;
using Jeuci.WeChatApp.Wechat.Models.Account;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Wechat.Models.BindEmail;
using Jeuci.WeChatApp.Wechat.Policy;

namespace Jeuci.WeChatApp.Wechat.Password.impl
{
    public class PassowrdProcessor : IPassowrdProcessor
    {
        private readonly IWechatAuthentManager _wechatAuthentManager;
        private readonly IRepository<UserInfo> _userRepository;
        private readonly IDirectEmailService _directEmailService;
        private const string m_retrievePwdCachePrefix = "RetrievePwd";

        public PassowrdProcessor(IWechatAuthentManager wechatAuthentManager, 
            IRepository<UserInfo> userRepository, 
            IDirectEmailService directEmailService)
        {
            _wechatAuthentManager = wechatAuthentManager;
            _userRepository = userRepository;
            _directEmailService = directEmailService;
        }

        [UnitOfWork]
        public bool ModifyPassword(JeuciAccount jeuciAccount,string newPassworld, out string urlOrMsg)
        {
            jeuciAccount.SynchronWechatUserInfo(_wechatAuthentManager);
            jeuciAccount.SynchronUserInfo(_userRepository);
            if (!jeuciAccount.IsExistAccount)
            {
                LogHelper.Logger.Error("获取用户个人信息失败");
                throw new Exception("获取您的个人信息失败，请与我们联系");
            }
            if (!jeuciAccount.IsValidPassword)
            {
                urlOrMsg = "您输入的密码不正确，请重新输入!";
                return false;
            }

            jeuciAccount.UserInfo.Password = newPassworld;
            try
            {
                _userRepository.Update(jeuciAccount.UserInfo);
                urlOrMsg = string.Format("/wechat/account/#/wechatforjeuci?isNeedCallBack=False&openId={0}", jeuciAccount.OpenId);
                return true;
            }
            catch (Exception e)
            {
                urlOrMsg = e.Message;
                return false;
            }
        }

        public Task<bool> SendRetrievePwdValidCode(string openId, string email)
        {
            var emailPolicy = new EmailPolicy();
            var validCode = emailPolicy.GetBindEmailValidCode(EmailValidCodeType.RetrievePwd);
            CacheHelper.SetCache(m_retrievePwdCachePrefix + openId, validCode);
            var emailBody = emailPolicy.GetEmailBody(validCode);
            return _directEmailService.SendValidCodeByEmail(email, emailBody, "彩盟网密码找回");
        }

        public bool RetrievePwd(JeuciAccount jeuciAccount, string newPassword,string validCodeStr, out string urlOrMsg)
        {
            jeuciAccount.SynchronWechatUserInfo(_wechatAuthentManager);
            jeuciAccount.SynchronUserInfo(_userRepository);
            if (!jeuciAccount.IsExistAccount)
            {
                LogHelper.Logger.Error("获取用户个人信息失败");
                throw new Exception("获取您的个人信息失败，请与我们联系");
            }

            var emialPolicy = new EmailPolicy();
            var validCode = CacheHelper.GetCache<EmailValidCode>(m_retrievePwdCachePrefix + jeuciAccount.OpenId);
            if (!emialPolicy.ValidEmailCode(validCode, validCodeStr,out urlOrMsg))
            {
                return false;
            }
            jeuciAccount.UserInfo.Password = newPassword;
            try
            {
                _userRepository.Update(jeuciAccount.UserInfo);
                urlOrMsg = string.Format("/wechat/account/#/wechatforjeuci?isNeedCallBack=False&openId={0}", jeuciAccount.OpenId);
                return true;
            }
            catch (Exception e)
            {
                urlOrMsg = e.Message;
                return false;
            }
        }
    }
}