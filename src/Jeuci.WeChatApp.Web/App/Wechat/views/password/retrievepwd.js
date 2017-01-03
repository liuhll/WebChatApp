(function() {
    angular.module("wechatApp").controller("wechatApp.views.retrievepwd", ["$scope", "$location", "$timeout","$interval", "Page", "Tips", "abp.services.app.wechatAuth", "abp.services.app.wechatAccount",
        function ($scope, $location, $timeout,$interval, page, tips, wechatAuthService, wechatAccount) {
        var vm = this;
        page.setTitle("找回密码");
        
        vm.isNeedCallBack = BoolHelper.parseBool($location.search().isNeedCallBack);            
        vm.openId = $location.search().openId;
        vm.user = {};
            debugger;
        //if (interval!==null && interval !== undefined) {
        //    $interval.cancel(interval);
        //    }
       
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
                    if (!result["data"]["isBindEmail"]) {
                        vm.obtainUserInfo = false;
                        vm.tipMessage = "您还没有绑定邮箱，请先绑定电子邮箱...";
                        location.href = window.location.origin + result["data"]["bindEmailAddress"];
                    } else {
                        vm.obtainUserInfo = true;
                        vm.user.accountName = getAccounName(result["data"].userInfo);
                        vm.user.openId = result["data"].openId;
                        vm.user.email = result["data"].userInfo.safeEmail;
                    }
                   

                } else {
                    vm.obtainUserInfo = false;
                    vm.tipMessage = "您还没有绑定掌盟专家账号，请先绑定掌盟专家账号...";
                    location.href = window.location.origin + result["data"]["bindWechatAddress"] + "?openId=" + result["data"]["openId"];
                }
            }).error(function (result) {
                console.log(JSON.stringfy(result));
            });
        }
        vm.getValidCode = function() {
            var longTime = 60;
            vm.obtainValidCode = true;
            vm.isAlreadyobtainValidCode = true;
            var interval = $interval(function () {
                longTime--;
                $("#btnObtainValidCode").text(longTime + "s后重新获取");
                if (longTime <= 0) {
                    $interval.cancel(interval);
                    vm.obtainValidCode = false;
                    $("#btnObtainValidCode").text("获取验证码");
                }
            }, 1000);
 
            wechatAccount.retrievePwdValidCode(vm.user.openId, vm.user.email).success(function (result) {
                
                if (result.code === 200) {
                    tips.isError = false;
                    tips.msg = result.data;
                    $timeout(function () {
                        TipHepler.ShowMsg();
                    }, 100);
                } else {
                    tips.msg = result.msg;
                    tips.isError = true;
                    $timeout(function () {
                        TipHepler.ShowMsg();
                    }, 100);
                }


            });
        }
        
        vm.confirm = function() {
            if (validUserLegality($timeout, vm.obtainValidCode, $scope.userForm, vm.user, tips)) {
                var newPassword = Encrypt.SHA256Encrypt(vm.user.newPassword);
                wechatAccount.retrievePwd({
                    openId: vm.user.openId,
                    newPassword: newPassword,
                    validCode: vm.user.validCode

                }).success(function (result) {
                    if (result["code"] === 200) {
                        showTips($timeout, result.msg, tips, false);
                        $timeout(function () {
                            location.href = location.origin + result.data;
                        }, 1000);
                    } else {
                        showTips($timeout, result.msg, tips);
                    }
                });
            }
        }
        }]);

    function getAccounName(userInfo) {
        if (userInfo.userName) {
            return userInfo.userName;
        }
        return userInfo.mobile;
    }

    function validUserLegality($timeout,obtainValidCode, userForm, user, tips) {

        if (!obtainValidCode) {
            showTips($timeout, "请先通过您添加的电子邮箱获取您的验证码！", tips);
            return false;
        }
        else if (userForm.validCode.$error.required) {
            showTips($timeout, "请输入您的邮箱验证码", tips);
            return false;
        }
        else if (userForm.newPassword.$error.required) {
            showTips($timeout, "请输入您的新密码", tips);
            return false;
        }

        else if (userForm.newPassword.$error.pattern) {
            showTips($timeout, "你要修改的密码长度不能小于六位", tips);
            return false;
        }
        else if (userForm.confirmNewPassword.$error.required) {
            showTips($timeout, "请确认您的新密码", tips);
            return false;
        }
        else if (userForm.confirmNewPassword.$modelValue !== userForm.newPassword.$modelValue) {
            showTips($timeout, "您输入的确认密码不正确，请重新输入", tips);
            user.confirmNewPassword = "";
            return false;
        }


        return true;
    }

    function showTips($timeout, msg, tips, isError) {
        isError = typeof isError !== 'undefined' ? isError : true;
        tips.isError = isError;
        tips.msg = msg;
        $timeout(function () {
            TipHepler.ShowMsg();
        }, 200);
    }
})();