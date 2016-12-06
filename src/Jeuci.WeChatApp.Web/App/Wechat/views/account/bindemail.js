(function() {
    angular.module("wechatApp").controller("wechatApp.views.bindemail", ["$scope", "$stateParams", "$timeout", "$interval", "Error", "Tips", "abp.services.app.wechatAccount", "abp.services.app.bindEmail",
        function ($scope, $stateParams, $timeout,$interval, error, tips, wechatAccount, bindEmail) {
            var vm = this;
           
            wechatAccount.getWechatUserInfo($stateParams.openId).success(function (result) {

                if (result.code === 200) {
                    vm.user = result.data;
                } else {
                    error.errorInfo = result.msg;
                    window.location.href = window.location.origin + "/wechat/account/#/error";
                }
            });
            
            vm.back = function() {
                history.go(-1);
            }

            vm.obtainValidCode = false;
           
            vm.getValidCode = function () {
                var msg = "";
                tips.isError = false;
                if ($scope.bindEmailForm.userEmail.$error.required) {
                    msg = "请输入您要绑定的电子邮箱";
                    tips.isError = true;
                } else if ($scope.bindEmailForm.userEmail.$error.pattern) {
                    msg = "请输入正确的电子邮箱账号";
                    tips.isError = true;
                }
                if (tips.isError) {
                    tips.msg = msg;
                    $timeout(function () {
                        TipHepler.ShowMsg();
                    }, 100);
                } else {
                    var longTime = 60;
                    vm.obtainValidCode = true;
                    var interval = $interval(function () {
                        longTime--;
                        $("#btnObtainValidCode").text(longTime + "s后重新获取");
                        if (longTime <= 0) {
                            $interval.cancel(interval);
                            vm.obtainValidCode = false;
                            $("#btnObtainValidCode").text("获取验证码");
                        }
                    }, 1000);
                    bindEmail.getValidCodeByEmail(vm.user.openId, vm.bindEmialModel.email).success(function (result) {
                        if (result.code === 200) {
                            tips.isError = false;
                            tips.msg = result.data;
                            $timeout(function () {
                                TipHepler.ShowMsg();
                            }, 100);
                    } else {
                            msg = result.msg;
                            tips.isError = true;
                            $timeout(function () {
                                TipHepler.ShowMsg();
                            }, 100);
                }
                   
                        
            });

        }
                   
    }

            vm.confirm = function () {
       
                if ($scope.bindEmailForm.$valid && vm.obtainValidCode) {

                } else {
                    var msg = "";
                    tips.isError = true;
                    if ($scope.bindEmailForm.userPassword.$error.required) {
                        msg = "请输入掌赢专家账号密码";
                    } else if ($scope.bindEmailForm.userEmail.$error.required) {
                        msg = "请输入您要绑定的电子邮箱";
                    } else if ($scope.bindEmailForm.userEmail.$error.pattern) {
                        msg = "请输入正确的电子邮箱账号";
                    } else if (!vm.obtainValidCode) {
                        msg = "请先通过您添加的电子邮箱获取您的验证码！";
                    }
                    else if ($scope.bindEmailForm.validCode.$error.required) {
                        msg = "请输入电子邮箱验证码";
                    }
                    tips.msg = msg;                  
                    $timeout(function() {
                        TipHepler.ShowMsg();
                    },100);
                   
                }
            }
}]);
})();