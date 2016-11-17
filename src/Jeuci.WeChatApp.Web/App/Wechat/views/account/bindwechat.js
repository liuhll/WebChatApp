(function() {
    angular.module('wechatApp').controller("wechatApp.views.bindwechat", ["$scope", "Page",function ($scope, page) {
        var vm = this;
        page.setTitle("绑定微信账号");
        vm.confirm = function () {
            if ($scope.userForm.$valid) {
                $scope.valid.errors =null;
                alert(JSON.stringify($scope.user));

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
})();