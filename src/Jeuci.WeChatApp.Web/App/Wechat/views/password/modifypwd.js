(function() {
    angular.module('wechatApp').controller('wechatApp.views.modifypwd', ["$scope","$location","$timeout", "Page","Tips", "abp.services.app.wechatAuth", "abp.services.app.wechatAccount",
        function ($scope,$location,$timeout, page,tips, wechatAuthService, wechatAccount) {
            var vm = this;
            page.setTitle("修改密码");
 
            vm.isNeedCallBack = BoolHelper.parseBool($location.search().isNeedCallBack);
            vm.openId = $location.search().openId;
            vm.user = {};
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
                        vm.obtainUserInfo = true;
                        vm.user.accountName = getAccounName(result["data"].userInfo);
                        vm.user.openId = result["data"].openId;
                    } else {
                        vm.obtainUserInfo = false;
                        vm.tipMessage = "您还没有绑定掌盟专家账号，请先绑定掌盟专家账号...";
                        location.href = window.location.origin + result["data"]["bindWechatAddress"] + "?openId=" + result["data"]["openId"];
                    }
                }).error(function (result) {
                    console.log(JSON.stringfy(result));
                });
            }

            vm.confirm = function () {
                if (validUserLegality($timeout, $scope.userForm, vm.user, tips)) {
                    var oldPassword = encryptPassword(vm.user.accountName, vm.user.oldPassword);
                    var newPassword = Encrypt.SHA256Encrypt(vm.user.newPassword);
                    wechatAccount.password({
                        openId: vm.user.openId,
                        accountName: vm.user.accountName,
                        oldPassword: oldPassword,
                        newPassworld: newPassword
                        },
                        {
                            method:"put"
                        })
                        .success(function(result) {
                            if (result["code"] === 200) {
                                showTips($timeout, result.msg, tips, false);
                                $timeout(function () {
                                    location.href = location.origin + result.data;
                                },1000);
                            } else {
                                showTips($timeout, result.msg, tips);
                            }
                        });
                }
            }


        }
    ]);

    function getAccounName(userInfo) {
        if (userInfo.userName) {
            return userInfo.userName;
        }
        return userInfo.mobile;
    }

    function validUserLegality($timeout,userForm,user, tips) {
        if (userForm.oldPassword.$error.required) {
            showTips($timeout, "请输入您掌赢专家账号的密码", tips);
            return false;
        }

        else if (userForm.newPassword.$error.required) {
            showTips($timeout, "请输入您要修改的新密码", tips);
            return false;
        }

        else if (userForm.newPassword.$error.pattern) {
            showTips($timeout, "你要修改的密码长度不能小于六位", tips);
            return false;
        }
        else if (userForm.confirmNewPassword.$error.required) {
            showTips($timeout, "请重复输入您要修改的新密码", tips);
            return false;
        }
        else if (userForm.confirmNewPassword.$modelValue !== userForm.newPassword.$modelValue) {
            showTips($timeout,"您输入的确认密码不正确，请重新输入", tips);
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

    function encryptPassword(nameStr, passwordStr) {
        var passwordStrSha256 = Encrypt.SHA256Encrypt(passwordStr);
        var privateSha256 = Encrypt.SHA256Encrypt(nameStr + passwordStrSha256);
        return privateSha256;
    }

})();