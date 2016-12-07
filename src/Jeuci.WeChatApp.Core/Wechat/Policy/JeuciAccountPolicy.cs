using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Logging;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Wechat.Models.Account;

namespace Jeuci.WeChatApp.Wechat.Policy
{
    public class JeuciAccountPolicy : ITransientDependency 
    {
        private JeuciAccount _account;
        private AccountOperateType _accountOperateType;

        public JeuciAccountPolicy(JeuciAccount account)
        {
            this._account = account;
            
        }

        public bool ValidAccountLegality(out string errorMsg)
        {
            switch (_account.AccountOperateType)
            {
                 case AccountOperateType.BindAccount:
                    return ValidBindAccountLegality(out errorMsg);

                 case AccountOperateType.BindEmail:
                    return ValidBindEmailLegality(out errorMsg);

                default:
                    errorMsg = "未知操作，请通过正确的途径进行操作";
                    LogHelper.Logger.Error("未知操作，用户可能不是通过正确的方式进行操作");
                    throw new Exception("未知操作，请通过正确的途径进行操作");

            }
          
       
        }

        private bool ValidBindEmailLegality(out string errorMsg)
        {
            if (!_account.IsExistAccount)
            {
                LogHelper.Logger.Error(string.Format("获取用户{0}个人信息失败，请与我们联系!", _account.Account));
                throw new Exception(string.Format("获取用户{0}个人信息失败，请与我们联系!", _account.Account));
            }
            if (!_account.IsValidPassword)
            {
                errorMsg = "您输入的密码错误，请确认您的密码!";
                return false;
            }
            if (_account.IsBindEmail)
            {
                errorMsg = "您已经绑定了电子邮箱，请不要重复绑定！";
                return false;
            }
            errorMsg = "OK";
            return true;
        }

        private bool ValidBindAccountLegality(out string errorMsg)
        {
            if (!_account.IsExistAccount)
            {
                errorMsg = string.Format("不存在{0}账号,请核对输入的账号是否正确!", _account.Account);
                return false;
            }
            if (_account.IsBindWechat)
            {
                errorMsg = "您已经绑定了掌盟账号，请不要重复绑定!";
                return false;
            }
            if (!_account.IsValidPassword)
            {
                errorMsg = "您输入的密码错误，请确认您的密码!";
                return false;
            }
            errorMsg = "OK";
            return true;
        }



        public bool ValidCanUnbindAccount(out string errorMsg)
        {
            if (_account.AccountOperateType == AccountOperateType.UnBindAccount)
            {
                if (!_account.IsExistAccount)
                {
                    throw new Exception("应用程序错误，系统没有查询到该账号信息，请与我们联系！");                 
                }
                if (!_account.IsValidPassword)
                {
                    errorMsg = "您输入的密码错误，请确认您的密码!";
                    return false;
                }
                errorMsg = "OK";
                return true;
            }
            errorMsg = "请确认您是从合法的途径进入解绑账号功能！";
            return false;
        }

        public bool BindWechatAccount(IRepository<UserInfo> userRepository, out string urlOrMsg)
        {
            _account.UserInfo.WeChat = _account.OpenId;
            _account.UserInfo.NickName = _account.UserWechatInfo.NickName;
            try
            {
                userRepository.Update(_account.UserInfo);
                urlOrMsg = string.Format("/wechat/account/#/wechatforjeuci?isNeedCallBack=false&openId={0}",_account.OpenId);
                return true;
            }
            catch (Exception e)
            {
                urlOrMsg = e.Message;
                return false;
            }
        
        }

        public bool UnBindWechatAccount(IRepository<UserInfo> userRepository, out string urlOrMsg)
        {
            _account.UserInfo.WeChat = null;
            _account.UserInfo.NickName = null;
            try
            {
                userRepository.Update(_account.UserInfo);
                urlOrMsg = string.Format("/wechat/account/#/bindwechat?openId={0}", _account.OpenId);
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
