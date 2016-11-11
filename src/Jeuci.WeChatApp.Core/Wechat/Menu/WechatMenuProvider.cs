using System;
using System.IO;
using Abp.Json;
using Jeuci.WeChatApp.Common.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP.Entities.Menu;

namespace Jeuci.WeChatApp.Wechat.Menu
{
    public class WechatMenuProvider : IWechatMenuProvider
    {
        private readonly string _webchatServiceAddress;
        public WechatMenuProvider()
        {
            _webchatServiceAddress = ConfigHelper.GetValuesByKey("WechatServiceAddress");
        }

        public ButtonGroup GetWechatMenu()
        {

            ButtonGroup bg = new ButtonGroup();

            var accountBtn = new SubButton("账户管理");


            accountBtn.sub_button.Add(new SingleViewButton()
            {
                name = "账号绑定",
                url = string.Format("{0}/account/bindingemail", _webchatServiceAddress)
            });

            accountBtn.sub_button.Add(new SingleViewButton()
            {
                name = "修改密码",
                url = string.Format("{0}/account/modifypwd", _webchatServiceAddress)
            });

            bg.button.Add(accountBtn);

            bg.button.Add(new SingleViewButton()
            {
                name = "玩法推荐",
                url = string.Format("{0}/play/recommend", _webchatServiceAddress)
            });

            return bg;
        }
    }
}