using System;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Logging;
using Jeuci.WeChatApp.Wechat.Authentication;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.Wechat.Password.impl
{
    public class PassowrdProcessor : IPassowrdProcessor
    {
        private readonly IWechatAuthentManager _wechatAuthentManager;
        private readonly IRepository<UserInfo> _userRepository;

        public PassowrdProcessor(IWechatAuthentManager wechatAuthentManager, 
            IRepository<UserInfo> userRepository)
        {
            _wechatAuthentManager = wechatAuthentManager;
            _userRepository = userRepository;
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
    }
}