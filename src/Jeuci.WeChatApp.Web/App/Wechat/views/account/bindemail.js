(function() {
    angular.module("wechatApp").controller("wechatApp.views.bindemail", ["$scope","$stateParams", "$timeout","Error","Tips","abp.services.app.wechatAccount","abp.services.app.bindEmail",
        function ($scope, $stateParams, $timeout, error, tips, wechatAccount, bindEmail) {
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
                bindEmail.getValidCodeByEmail(vm.user.openId, vm.bindEmialModel.email).success(function(result) {
                    vm.obtainValidCode = true;
                    console.log(result);

                    $timeout(function() {
                        vm.obtainValidCode = false;
                    },10000);
                });
                
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