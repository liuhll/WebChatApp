(function() {
    angular.module('wechatApp').controller("wechatApp.views.bindwechat",
        ["$scope", "Page", "abp.services.app.wechatAuth", function ($scope, page, wechatAuth) {
        var vm = this;
        page.setTitle("绑定微信账号");
        vm.confirm = function () {

            if ($scope.userForm.$valid) {

                var user = {};
                $scope.valid.errors = null;

                var redirectUrl = window.location.origin + "/api/services/app/wechatAuth/GetWechatUserInfo",
                    oauth2Uri = "",
                    oAuthScope = "0",
                    state = "Jeuci-" + new Date().getTime();//随机数，用于识别请求可靠性
                debugger;
                wechatAuth.getWechatAuthorizeUrl(redirectUrl, state, oAuthScope).success(function (result) {

                    alert(result);

                  /*  user["password"] = encryptPassword($scope.user.account, $scope.user.password);
                    user["account"] = $scope.user.account;
                    alert(JSON.stringify(user));*/
                }).error(function(e) {
                    alert(JSON.stringify(e));
                });

             

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
