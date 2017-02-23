(function() {
    angular.module('wechatApp').controller("wechatApp.views.bindwechat",
        ["$scope", "$location","$timeout", "Page", "Error", "Tips", "abp.services.app.wechatAccount",
            function ($scope, $location,$timeout,page, error, tips, wechatAccount) {
        var vm = this;
        page.setTitle("绑定微信账号");

        var openId = $location.search().openId;
            vm.showBindForm = true;
        if (openId === null || openId === undefined || openId === "" ) {
            vm.showBindForm = false;
            error.errorInfo = "请从正规的途经进入该页面,您没有绑定账号的权限！";
            return false;
        }
        vm.openId = openId;
        vm.confirm = function () {
            if ($scope.userForm.$valid) {
                var userInfo = {};
                userInfo["account"] = $scope.user.account.toLocaleLowerCase();
                userInfo["password"] = $scope.user.password;
                userInfo["openId"] = vm.openId;
                userInfo.password = encryptPassword(userInfo.account,userInfo.password);
                wechatAccount.bindWechatAccount(userInfo)
                    .success(function (result) {
                        if (result["code"] !== 200) {
                            tips.msg = result["msg"];
                            tips.isError = true;
                            TipHepler.ShowMsg();
                        } else {
                            tips.isError = false;
                            tips.msg = result["msg"];
                            TipHepler.ShowMsg();
                            $timeout(function() {                                
                                window.location.href = window.location.origin + result["data"];
                            },1000);                           
                        }                      
                    });
            } else {
                tips.isError = true;
                TipHepler.ShowMsg();
            }
        }           
        }]);

    function encryptPassword(nameStr, passwordStr) {
        var passwordStrSha256 = Encrypt.SHA256Encrypt(passwordStr);
        var privateSha256 = Encrypt.SHA256Encrypt(nameStr + passwordStrSha256);
        return privateSha256;
    }
})();
