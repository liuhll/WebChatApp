using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Web.Controllers;
using Jeuci.WeChatApp.WeChatAuth;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

namespace Jeuci.WeChatApp.Web.Areas.Wechat.Controllers
{
    public class AccountController : WeChatAppControllerBase
    {

        private readonly IWechatAuthAppService _wechatAuthAppService;

       // private const string base_returnUrl = "http://{0}{1}";

        public AccountController(IWechatAuthAppService wechatAuthAppService)
        {
            _wechatAuthAppService = wechatAuthAppService;
           
        }

        public ActionResult Index()
        {
            return View("~/App/Wechat/views/layout/layout.cshtml");
        }

        /// <summary>
        /// 获取用户的基本信息，需要 base_scope的值为1
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult UserInfoCallback(string code, string state, string returnUrl)
        {
            var base_returnUrl = "http://{0}{1}";
            if (string.IsNullOrEmpty(code))
            {
                // 错误页面1，提示用户应当授予权限
                return Redirect(string.Format(base_returnUrl, Request.Url.Host, "/account/#/errorinfo?code=noauth"));
            }
            //if (state != CacheHelper.GetCache<string>("State"))
            //{
            //    //错误页面2，提示用户应当从正规途径进入
            //    return Redirect(string.Format(base_returnUrl, Request.Url.Host, "/account/#/errorinfo?code=nostate"));
            //}
            var userInfoResult = _wechatAuthAppService.GetWechatUserInfo(code, state);
            //if (userInfoResult.Code != ResultCode.Success)
            //{
            //    //错误页面3，获取用户基本信息失败
            //    return Redirect(string.Format(base_returnUrl, Request.Url.Host, "/account/#/errorinfo?code=fail_get_userinfo"));
            //}
            //ViewBag.UserInfo = userInfoResult.Data;
            //ViewBag.IsNeedCallBack = false;

            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = string.Format(base_returnUrl, Request.Url.Host, "/wechat/account/#/bindwechat");
            }
            returnUrl = string.Format(returnUrl.Contains("?") ? "{0}&isNeedCallBack={1}&openId={2}" : "{0}?isNeedCallBack={1}&openId={2}", returnUrl, false, userInfoResult.Data.OpenId);
            Logger.Info("回调的url:"+returnUrl);
            return Redirect(returnUrl);
        }


        /// <summary>
        /// 获取用户的OpenId基本信息 base_scope的值为0
        /// </summary>
        public ActionResult UserBaseCallback(string code, string state, string returnUrl)
        {
            var base_returnUrl = "http://{0}{1}";
            if (string.IsNullOrEmpty(code))
            {
                // 错误页面1，提示用户应当授予权限
                return Redirect(string.Format(base_returnUrl, Request.Url.Host, "/account/#/errorinfo?code=noauth"));
            }
            if (state != CacheHelper.GetCache<string>("State"))
            {
                //错误页面2，提示用户应当从正规途径进入
                return Redirect(string.Format(base_returnUrl, Request.Url.Host, "/account/#/errorinfo?code=nostate"));
            }
            var userInfoResult = _wechatAuthAppService.GetWechatUserOpenId(code, state);
            if (userInfoResult.Code != ResultCode.Success)
            {
                //错误页面3，获取用户基本信息失败
                return Redirect(string.Format(base_returnUrl, Request.Url.Host, "/account/#/errorinfo?code=fail_get_userinfo"));
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = string.Format(base_returnUrl, Request.Url.Host, "/wechat/account/#/bindwechat");
            }

            // returnUrl = string.Format(returnUrl.Contains("?") ? "{0}&isNeedCallBack={1}&openId={2}" : "{0}?isNeedCallBack={1}&openId={2}", returnUrl, false, userInfoResult.Data);
            returnUrl = GetCallBackUrl(returnUrl, userInfoResult.Data);
            Logger.Info("回调的url:" + returnUrl);
            return Redirect(returnUrl);
        }

        private string GetCallBackUrl(string url,string openId)
        {
            if (url.Contains("?"))
            {
                if (url.ToLower().Contains("isneedcallback"))
                {
                    url = Regex.Replace(url, "isNeedCallBack", "isNeedCallBack=false", RegexOptions.IgnoreCase);
                }
                else
                {
                    url += "&isNeedCallBack=false";
                }
            }
            else
            {
                url += "?isNeedCallBack=false";
            }
            url += "&openId=" + openId;
            return url;
        }
    }
}