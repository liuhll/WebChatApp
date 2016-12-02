(function() {
    angular.module('wechatApp').controller("app.views.account.unbindAccount", ["$scope", "$timeout", "$modalInstance", "abp.services.app.wechatAccount", "Tips", "account",
        function ($scope, $timeout, $modalInstance, wechatAccount, tips, account) {
            var vm = this;
            vm.accountName = account.accountName;

            vm.confirm = function () {
                $modalInstance.result = $modalInstance.result || {};     
                if (!$scope.accountForm.$valid) {                                  
                    tips.isError = true;
                    tips.msg = "请输入掌盟专家账号密码";       
                    $timeout(function() {
                        TipHepler.ShowMsg();
                    },100);
                    $modalInstance.result.success = false;
                    return false;
                }
              
                var password = encryptPassword(vm.accountName, vm.password);
                wechatAccount.unbindWechatAccount({
                    openId:account.openId,
                    account: vm.accountName,
                    password:password
                }).success(function (result) {
                    if (result["code"] === 200) {
                        tips.isError = false;
                        $modalInstance.result.success = true;
                        $modalInstance.result.callbackUrl = result.data;
                        $modalInstance.result.msg = result.msg;
                        $modalInstance.close();
                    } else {
                        tips.isError = true;
                        tips.msg = result.msg;
                        $timeout(function () {
                            TipHepler.ShowMsg();
                        }, 100);
                        $modalInstance.result.success = false;
                        return false;
                    }
                });

            };

            vm.cancel = function () {
                $modalInstance.dismiss();
            };
        }
    ]);

    function encryptPassword(nameStr, passwordStr) {
        var passwordStrSha256 = Encrypt.SHA256Encrypt(passwordStr);
        var privateSha256 = Encrypt.SHA256Encrypt(nameStr + passwordStrSha256);
        return privateSha256;
    }
})();