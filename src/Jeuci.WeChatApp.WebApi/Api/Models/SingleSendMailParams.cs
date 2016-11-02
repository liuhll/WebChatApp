using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;

namespace Jeuci.WeChatApp.Api.Models
{
    public class SingleSendMailParams : ValidationParams
    {

        public string ToAddress { get; set; }

        public EmailBodyType TmailBodyType { get; set; }

        public string EmailTemplet { get; set; }

        public EmailTemplet? EmailTempletService {
            get
            {
                if (!string.IsNullOrEmpty(EmailTemplet))
                {
                    return ConvertHelper.StringToEnum<EmailTemplet>(EmailTemplet);
                }
                return null;
            }
        }

    }
}