(function() {
    angular.module('planApp').controller('wechatApp.views.rechargefreelist', ["$location", "$state", "$window",
        "Page", "Tips", "abp.services.app.wechatAuth", "abp.services.app.wechatAccount", "abp.services.app.recharge",
        function ($location, $state, $window, page, tips, wechatAuthService, wechatAccount, recharge) {
            var vm = this;
            page.setTitle("在线充值");
            vm.isNeedCallBack = BoolHelper.parseBool($location.search().isNeedCallBack);
            vm.openId = $location.search().openId;
            if (vm.isNeedCallBack) {
                var oAuthScope = "0",
                     state = "Jeuci-" + new Date().getTime();
                var redirectUrl = window.location.origin + "/wechat/account/userBaseCallback?returnurl=" + encodeURIComponent(location.href);
                vm.tipMessage = "请稍等...正在获取用户信息";
                wechatAuthService.getWechatAuthorizeUrl(redirectUrl, state, oAuthScope).success(function (result) {
                    location.href = result;
                }).error(function (result) {
                    console.log(JSON.stringfy(result));
                });
            } else {
                wechatAccount.getWechatUserInfo(vm.openId).success(function (result) {
                    if (result["data"]["isBindWechat"]) {
                        vm.showUserInfo = true;
                        vm.jeuciAccount = result["data"];
                        recharge.getFreeList().success(function(result1) {
                            if (result1.code === 200) {
                                vm.freeList = result1.data;
                                vm.rechargeAcount = 0;
                                vm.otherAccount = 0;
                                vm.selectOtherItem = false;
                            } else {
                                alert(result1.msg);
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

            vm.rechargeAmount = function(amount) {
                vm.rechargeAcount = amount;
                vm.inputRechargeValue = 0;
                vm.selectOtherItem = false;
            }
            vm.selectOther = function () {               
                vm.selectOtherItem = true;                
                vm.inputRechargeValue = 0;
                vm.rechargeAcount = vm.inputRechargeValue;
            }
            vm.inputRecharge = function () {              
                vm.rechargeAcount = vm.inputRechargeValue;
            }

            vm.purchase = function() {
                if (vm.rechargeAcount === null || vm.rechargeAcount === undefined || vm.rechargeAcount === 0) {
                    alert("请确认你要充值的金额");
                    return;
                }
                if (vm.rechargeAcount % 100 !==0) {
                    alert("您要充值的金额必须是100的整数");
                    return;
                }
                recharge.getUnifiedOrder(vm.openId, vm.rechargeAcount,
                {
                    beforesend: function() {
                        $.showLoading("正在生成订单，请稍等...");
                    },
                    complete: function() {
                        $.hideLoading();
                    }
                }).success(function (result) {
                    if (result.code === 200) {
                        $state.go("unifiedOrder", result.data);
                    } else {
                        alert(result.msg);
                    }
                }).error(function (result) {
                    alert("订单生成失败，请稍后重试!");
                });;
            }
        }
    ]);
})();