(function() {
    var app = angular.module("wechatApp");
    app.controller("wechatApp.views.wechatforjeuci", ["$location", "$http","$modal","$timeout",
        "Page","Tips", "abp.services.app.wechatAuth","abp.services.app.wechatAccount",
        function ($location, $http, $modal,$timeout, page, tips, wechatAuthService, wechatAccount) {
            var vm = this;
            page.setTitle("账号信息");
            vm.isNeedCallBack = BoolHelper.parseBool($location.search().isNeedCallBack);
            vm.openId = $location.search().openId;
 
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

            vm.unbindAccount = function () {
                var modalInstance = $modal.open({
                    templateUrl: '/App/Wechat/views/account/parts/unbindAccountDialog.cshtml',
                    controller: 'app.views.account.unbindAccount as vm',
                    backdrop: 'static',
                    resolve: {
                        account: function() {
                            return {
                                openId: vm.jeuciAccount.openId,
                                accountName: getJeuciAccountName(vm.jeuciAccount.userInfo)
                        };
                        }
                    },
                });

                modalInstance.result.then(function () {

                    tips.isError = !modalInstance.result.success;
                    tips.msg = modalInstance.result.msg;
                    TipHepler.ShowMsg();
                    $timeout(function() {
                        window.location.href = window.location.origin + modalInstance.result.callbackUrl;
                    },1000);

                });

            }
        }
    ]);

    function getJeuciAccountName(userInfo) {    
        if (userInfo.userName) {
            return userInfo.userName;
        }
        return userInfo.mobile;
    }
})();