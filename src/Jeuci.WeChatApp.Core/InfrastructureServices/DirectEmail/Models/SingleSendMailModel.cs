using System;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;

namespace Jeuci.WeChatApp.InfrastructureServices.DirectEmail.Models
{
    public class SingleSendMailModel
    {
        private readonly string _toAddress;

        private readonly EmailTemplet _emailTemplet;

        private readonly EmailBodyType _emailBodyType;

        /// <summary>
        /// 单一发信接口模型
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="emailBodyType"></param>
        /// <param name="emailTemplet"></param>
        public SingleSendMailModel(string toAddress, EmailBodyType emailBodyType = EmailBodyType.Html, EmailTemplet? emailTemplet = null)
        {
            _toAddress = toAddress;
            _emailBodyType = emailBodyType;
            _emailTemplet = emailTemplet ?? GetDefaultEmailTemplet();
        }

        private EmailTemplet GetDefaultEmailTemplet()
        {
            var defaultEmaiTemplet = ConfigHelper.GetValuesByKey("DefaultEmaiTemplet");
            return ConvertHelper.StringToEnum<EmailTemplet>(defaultEmaiTemplet);
        }

        public string AccountName {
            get { return ConfigHelper.GetValuesByKey("EmailAccountName"); }
        }

        public bool ReplyToAddress
        {
            get { return true; }
        }

        public int AddressType
        {
            get { return 0; }
        }

        public string TagName
        {
            get { return ConfigHelper.GetValuesByKey("EmailTagName"); }
        }

        public string ToAddress
        {
            get { return _toAddress; }
        }

        public string Subject
        {
            get { return ConfigHelper.GetValuesByKey(_emailTemplet.ToString() + "Subject"); }
        }

        public string FromAlias
        {
            get { return ConfigHelper.GetValuesByKey("FromAlias"); }
        }

        public string EMailBody
        {
            get
            {
                var emailTempletManager = EmailTempletManager.GetEmailTempletManager();
                return emailTempletManager.GetEmailTemplet(_emailTemplet, _emailBodyType);
            }
        }

        public EmailBodyType EmailBodyType {
            get { return _emailBodyType; }
        }

    }
}