using System;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Logging;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Wechat.Authentication;
using Senparc.Weixin;
using Senparc.Weixin.MP.CommonAPIs;

namespace Jeuci.WeChatApp.Wechat.Models.Account
{
    public class JeuciAccount
    {
        private readonly string m_openId;
        private readonly string m_account;
        private readonly string m_password;

        private UserInfo _userInfo;

        internal bool IsExistAccount {
            get { return _userInfo != null; }
        }

       // internal bool IsBindAction { get; private set; }

         internal AccountOperateType AccountOperateType { get; }


        internal bool IsValidPassword
        {
            get
            {
                var acccountName = !string.IsNullOrEmpty(_userInfo.UserName)
                    ? _userInfo.UserName
                    : _userInfo.Mobile;

                return m_password.Equals(EncryptionHelper.EncryptSHA256(acccountName + _userInfo.Password));
            }
        }

        public JeuciAccount(string openId, AccountOperateType accountOperateType)
        {
            m_openId = openId;
            AccountOperateType = accountOperateType;
        }

        public JeuciAccount(string openId, string account, string userPassword, AccountOperateType accountOperateType) : this(openId, accountOperateType)
        {
            m_account = account;
            m_password = userPassword;         
        }

        public string Account
        {
            get { return m_account; }
        }

        public bool IsBindWechat {
            get
            {
                return !string.IsNullOrEmpty(_userInfo?.WeChat);
            }
        }

        public bool IsBindEmail
        {
            get
            {
                return !string.IsNullOrEmpty(_userInfo?.SafeEmail);
            }
        }

        public string OpenId
        {
            get { return UserWechatInfo.OpenId; }
        }

        public string BindWechatAddress
        {
            get
            {
                return "/wechat/account/#/bindwechat";
            }
        }

        public string BindEmailAddress
        {
            get { return string.Format("/wechat/account/#/bindemail/{0}",this.OpenId); }
        }

        public UserInfo UserInfo {
            get
            {
                if (_userInfo == null) throw new Exception("获取用户信息失败");
                if (AccountOperateType !=AccountOperateType.ObtainAccount
                    && AccountOperateType != AccountOperateType.ModifyPassword
                    && AccountOperateType != AccountOperateType.RetrievePwd
                    && !IsValidPassword)
                {
                    throw new Exception("您输入密码错误，无法获取用户信息");
                }
                return _userInfo;
            }
        }

        public WechatAccount UserWechatInfo { get; private set; }

        /// <summary>
        /// 从微信服务端同步用户的个人基本信息
        /// </summary>
        /// <param name="wechatAuthentManager"></param>
        public void SynchronWechatUserInfo(IWechatAuthentManager wechatAuthentManager)
        {
            if (string.IsNullOrEmpty(m_openId))
            {
                LogHelper.Logger.Error("OpenId为空，用户可能不是从合法的途径进入到该站点!");
                throw new Exception("OpenId不能为空，请从合法的途径进入该站点");
            }
            var userInfoResult = CommonApi.GetUserInfo(wechatAuthentManager.GetAccessToken(), m_openId);
            if (userInfoResult.errcode != ReturnCode.请求成功)
            {
                LogHelper.Logger.Error("请求用户的基本信息失败，原因:" + userInfoResult.errcode);
                throw new Exception("请求用户的基本信息失败，原因:" + userInfoResult.errcode);
            }
            UserWechatInfo = userInfoResult.MapTo<WechatAccount>();
        }

        /// <summary>
        /// 从公司数据库同步个人信息
        /// </summary>
        /// <param name="userRepository"></param>
        public void SynchronUserInfo(IRepository<UserInfo> userRepository)
        {
            if (string.IsNullOrEmpty(m_openId))
            {
                LogHelper.Logger.Error("OpenId为空，用户可能不是从合法的途径进入到该站点!");
                throw new Exception("OpenId不能为空，请从合法的途径进入该站点");
            }

            _userInfo = userRepository.FirstOrDefault(p => p.WeChat == m_openId);
            if (_userInfo == null)
            {
                //如果用户名和密码不为空的情况下就任务是绑定行为
                if (!string.IsNullOrEmpty(m_account) && !string.IsNullOrEmpty(m_password))
                {
                    _userInfo = userRepository.FirstOrDefault(p => p.UserName == m_account || p.Mobile == m_account);
                }
                
            }


        }
    }
}
