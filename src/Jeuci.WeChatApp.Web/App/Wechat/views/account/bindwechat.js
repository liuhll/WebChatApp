(function() {
    angular.module('wechatApp').controller("wechatApp.views.bindwechat",
        ["$scope", "$location", "Page", "Error", "Valid", "abp.services.app.wechatAccount",
            function ($scope, $location, page, error, valid, wechatAccount) {
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
                userInfo["account"] = $scope.user.account;
                userInfo["password"] = $scope.user.password;
                userInfo["openId"] = vm.openId;
                userInfo.password = encryptPassword(userInfo.account,userInfo.password);
                wechatAccount.bindWechatAccount(userInfo)
                    .success(function (result) {
                        if (result["code"] !== 200) {
                            valid.errors = result["msg"];
                            showError();
                        } else {
                            alert("绑定成功!");
                            window.location.href = window.location.origin + result["data"];
                        }
                       
                    });
            } else {
                showError();
            }
        }
           
        }]);

    function showError() {
        var $tooltips = $('.js_tooltips');
        if ($tooltips.css('display') !== 'none') return;
        $('.page.cell').removeClass('slideIn');
        $tooltips.css('display', 'block');
        setTimeout(function () {
            $tooltips.css('display', 'none');
        }, 2000);
    }

    function encryptPassword(nameStr, passwordStr) {
        var passwordStrSha256 = Encrypt.SHA256Encrypt(passwordStr);
        var privateSha256 = Encrypt.SHA256Encrypt(nameStr + passwordStrSha256);
        return privateSha256;
    }
})();
