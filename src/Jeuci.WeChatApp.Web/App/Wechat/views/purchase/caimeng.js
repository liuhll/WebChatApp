(function () {
    angular.module('planApp').controller('wechatApp.views.caimeng', ["$location","$state",
        "Page", "Tips", "abp.services.app.wechatAuth", "abp.services.app.wechatAccount", "abp.services.app.lottery", function ($location,$state, page, tips, wechatAuthService, wechatAccount, lottery) {
            var vm = this;
            page.setTitle("购买服务");
            vm.isNeedCallBack = BoolHelper.parseBool($location.search().isNeedCallBack);
            vm.openId = $location.search().openId;
 
            if (vm.isNeedCallBack) {
                var oAuthScope = "0",
                     state = "Jeuci-" + new Date().getTime();
                var redirectUrl = window.location.origin + "/wechat/account/userBaseCallback?returnurl=" + encodeURIComponent(location.href);
                vm.tipMessage = "请稍等...正在获取用户信息";
                wechatAuthService.getWechatAuthorizeUrl(redirectUrl, state, oAuthScope).success(function (result) {
                    location.href = result;
                }).error(function(result) {
                    console.log(JSON.stringfy(result));
                });
            } else {
                wechatAccount.getWechatUserInfo(vm.openId).success(function (result) {
                    if (result["data"]["isBindWechat"]) {
                        vm.showUserInfo = true;                        
                        vm.jeuciAccount = result["data"];

                        lottery.getServiceList(null, {
                            beforeSend: function () {
                                $.showLoading("正在加载...");
                            },
                            complete: function () {
                                $.hideLoading();
                            }
                        }).success(function (result1) {
                            if (result1.code === 200) {
                                vm.cpTypeList = result1.data;
                            } else {
                                error.errorInfo = result1.msg;
                                window.location.href = window.location.origin + "/wechat/account/#/error";
                            }
                        });


                    } else {
                        vm.showUserInfo = false;
                        vm.tipMessage = "您还没有绑定掌盟专家账号，请先绑定掌盟专家账号...";
                        location.href = window.location.origin + result["data"]["bindWechatAddress"] + "?openId=" + result["data"]["openId"];
                    }
                }).error(function (result) {
                    console.log(JSON.stringfy(result));
                });
            }

            vm.getServiceList = function(sid, openId) {
                $state.go("serviceList", { sid: sid, openId: openId });
            }


        }]);
})();