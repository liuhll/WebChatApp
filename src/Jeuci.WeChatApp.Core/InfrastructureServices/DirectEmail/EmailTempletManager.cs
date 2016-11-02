using System;
using System.IO;
using Abp.Dependency;
using Abp.Logging;
using Jeuci.WeChatApp.Common;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Exceptions;
using Jeuci.WeChatApp.Common.Tools;

namespace Jeuci.WeChatApp.InfrastructureServices.DirectEmail
{
    public class EmailTempletManager : ISingletonDependency
    {
        private readonly bool _isCacheEmailTemplet;

        private readonly TimeSpan _cacheTimeOut;

        private static readonly EmailTempletManager _emailTempletManager = new EmailTempletManager();

        private EmailTempletManager()
        {
            var cacheTimeInt = ConfigHelper.GetIntValues("CacheEmailTempletTimeOut");
            _cacheTimeOut = TimeSpan.FromMinutes(cacheTimeInt);
            _isCacheEmailTemplet = ConfigHelper.GetBoolValues("IsCacheEmailTemplet") && cacheTimeInt > 0;
        }

        public static EmailTempletManager GetEmailTempletManager()
        {
            return _emailTempletManager;
        }

        public string GetEmailTemplet(EmailTemplet emailTemplet, EmailBodyType emailBodyType)
        {
            var emailTempletName = string.Format("{0}.{1}", emailTemplet, emailBodyType);
            string emailTempletStr;
            if (_isCacheEmailTemplet)
            {
                emailTempletStr = CacheHelper.GetCache<string>(emailTempletName);
                if (string.IsNullOrEmpty(emailTempletStr))
                {
                    return GetEmailTempletFromFile(emailTempletName);
                }
                return emailTempletStr;
            }
            return GetEmailTempletFromFile(emailTempletName);
        }

        private string GetEmailTempletFromFile(string emailTempletName)
        {
            string emailTempletPath = string.Format(@"{0}{1}\{2}",AppDomain.CurrentDomain.BaseDirectory, @"EmailTemplet", emailTempletName);
            string emailTempletStr = string.Empty;
            try
            {
                emailTempletStr = File.ReadAllText(emailTempletPath);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(string.Format(MessageTips.ReadEmailTempletError,ex.Message));
                throw new ReadEmailTempletException(string.Format(MessageTips.ReadEmailTempletError, ex.Message),ex);
            }
            if (string.IsNullOrEmpty(emailTempletStr))
            {
                LogHelper.Logger.Error(string.Format(MessageTips.ReadEmailTempletError, string.Format("读取到的{0}为空", emailTempletName)));
                throw new Exception(string.Format(MessageTips.ReadEmailTempletError, string.Format("读取到的{0}为空",emailTempletName)));

            }
            if (_isCacheEmailTemplet)
            {
                CacheHelper.SetCache(emailTempletName,emailTempletStr,_cacheTimeOut);
            }
            return emailTempletStr;

        }
    }
}