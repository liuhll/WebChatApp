(function() {
    angular.module('wechatApp').controller("wechatApp.views.bindwechat",
        ["$scope", "$location", "Page","Error", "abp.services.app.wechatAuth", function ($scope, $location, page,error, wechatAuth) {
        var vm = this;
        page.setTitle("绑定微信账号");
            debugger;
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

            } else {           
                var $tooltips = $('.js_tooltips');
                if ($tooltips.css('display') !== 'none') return;
                $('.page.cell').removeClass('slideIn');
                $tooltips.css('display', 'block');
                setTimeout(function () {
                    $tooltips.css('display', 'none');
                }, 2000);
            }
        }
           
        }]);


    function encryptPassword(nameStr, passwordStr) {
        var passwordStrSha256 = Encrypt.SHA256Encrypt(passwordStr);
        var privateSha256 = Encrypt.SHA256Encrypt(nameStr + passwordStrSha256);
        return privateSha256;
    }
})();
