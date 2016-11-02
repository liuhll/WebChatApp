using Abp.Logging;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dm.Model.V20151123;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.InfrastructureServices.DirectEmail.Models;

namespace Jeuci.WeChatApp.InfrastructureServices.DirectEmail
{
    public class DirectEmailService : IDirectEmailService
    {
        public bool SingleSendMail(SingleSendMailModel model)
        {
            var regionId = ConfigHelper.GetValuesByKey("EmailRegionId");
            var accessKeyId = ConfigHelper.GetValuesByKey("EmailAccessKeyId");
            var secret = ConfigHelper.GetValuesByKey("EmailSecret");
            IClientProfile profile = DefaultProfile.GetProfile(regionId, accessKeyId, secret);
            IAcsClient client = new DefaultAcsClient(profile);
            SingleSendMailRequest request = new SingleSendMailRequest();
            try
            {
                request.AccountName = model.AccountName;
                request.FromAlias = model.FromAlias;
                request.AddressType = model.AddressType;
                request.TagName = model.TagName;
                request.ReplyToAddress = model.ReplyToAddress;
                request.ToAddress = model.ToAddress;
                request.Subject = model.Subject;
                if (model.EmailBodyType == EmailBodyType.Html)
                {
                    request.HtmlBody = model.EMailBody; 
                }
                else
                {
                    request.TextBody = model.EMailBody;
                }
                SingleSendMailResponse httpResponse = client.GetAcsResponse(request);
                return httpResponse.HttpResponse.Status == 200;
            }
            catch (ServerException e)
            {
                LogHelper.Logger.Error("发送邮件失败，服务器原因，失败原因："+e.Message,e);
                return false;
            }
            catch (ClientException e)
            {
                LogHelper.Logger.Error("发送邮件失败，客服端原因，失败原因：" + e.Message, e);
                return false;
            }
        }
    

        public bool BatchSendMail()
        {
            throw new System.NotImplementedException();
        }
    }
}