﻿using System;
using System.IO;
using Abp.Json;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Configs;
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
                name = "我的账号",
                url = string.Format("{0}/wechat/account/#/wechatforjeuci?isNeedCallBack", _webchatServiceAddress)
            });

            accountBtn.sub_button.Add(new SingleViewButton()
            {
                name = "找回密码",
                url = string.Format("{0}/wechat/account/#/retrievepwd?isNeedCallBack", _webchatServiceAddress)
            });

            accountBtn.sub_button.Add(new SingleViewButton()
            {
                name = "修改密码",
                url = string.Format("{0}/wechat/account/#/modifypwd?isNeedCallBack", _webchatServiceAddress)
            });

            accountBtn.sub_button.Add(new SingleViewButton()
            {
                name = "代理充值",
                url = string.Format("{0}/wechat/recharge/#/freelist?isNeedCallBack",_webchatServiceAddress)
            });
            accountBtn.sub_button.Add(new SingleClickButton()
            {
                name = "帮助",
                key = WeChatConfig.MENU_SUB_CLICK_HELP,
                type = "click"
            });

            bg.button.Add(accountBtn);

            var purchaseServiceBtn = new SubButton("购买软件");
            purchaseServiceBtn.sub_button.Add(new SingleViewButton()
            {
                name = "掌赢专家",
                url = string.Format("{0}/wechat/purchase/#/caimeng?isNeedCallBack", _webchatServiceAddress)
            });
            bg.button.Add(purchaseServiceBtn);

            //var addedServiceBtn = new SubButton("更多服务");

            //addedServiceBtn.sub_button.Add(new SingleViewButton()
            //{
            //    name = "免费计划",
            //    url = string.Format("{0}/wechat/plan/#/free", _webchatServiceAddress)
            //});

            bg.button.Add(new SingleViewButton()
            {
                name = "免费计划",
                url = string.Format("{0}/wechat/plan/#/free", _webchatServiceAddress)
            });

            
           // bg.button.Add(addedServiceBtn);
              
            return bg;
        }
    }
}