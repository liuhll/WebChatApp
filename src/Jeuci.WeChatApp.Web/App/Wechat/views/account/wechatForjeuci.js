(function() {
    var app = angular.module("wechatApp");
    app.controller("wechatApp.views.wechatforjeuci", ["$location", "$http",
        "Page", "abp.services.app.wechatAuth","abp.services.app.wechatAccount",
        function ($location, $http, page, wechatAuthService, wechatAccount) {
            debugger;
            var vm = this;
            page.setTitle("账号信息");
            vm.isNeedCallBack = BoolHelper.parseBool($location.search().isNeedCallBack);
            vm.openId = $location.search().openId;
            console.log(vm.isNeedCallBack);
            if (vm.isNeedCallBack) {
                var oAuthScope = "0",
                     state = "Jeuci-" + new Date().getTime();
                var redirectUrl = window.location.origin + "/wechat/account/userBaseCallback?returnurl=" + encodeURIComponent(location.href);
                vm.tipMessage = "请稍等...正在获取用户信息";
                wechatAuthService.getWechatAuthorizeUrl(redirectUrl, state, oAuthScope).success(function (result) {
                    location.href = result;
                });
            } else {
                wechatAccount.getWechatUserInfo(vm.openId).success(function (result) {
                    if (result["data"]["isBindWechat"]) {
                        vm.showUserInfo = true;                        
                        vm.jeuciAccount = result["data"];

                    } else {
                        vm.showUserInfo = false;
                        vm.tipMessage = "您还没有绑定掌盟专家账号，请先绑定掌盟专家账号...";
                        location.href = window.location.origin + result["data"]["bindWechatAddress"] + "?openId=" + result["data"]["openId"];
                    }
                });
            }
        }
    ]);
})();