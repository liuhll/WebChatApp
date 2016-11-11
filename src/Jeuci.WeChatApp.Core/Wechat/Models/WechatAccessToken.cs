using System;

namespace Jeuci.WeChatApp.Wechat.Models
{
    [Obsolete("第三方框架已经实现了自动获取accesstoken的功能，无需自己实现")]
    public class WechatAccessToken
    {
        private const int m_bufferTime = 60;

        public string AccessToken { get; set; }

        public DateTime CreateDateTime { get; set; }

        public int ExpiresIn { get; set; }

        public bool IsValidToken
        {
            get { return CreateDateTime.AddSeconds(ExpiresIn - m_bufferTime) <= DateTime.Now; }
        }
    }
}