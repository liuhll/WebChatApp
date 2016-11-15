(function() {
    angular.module('wechatApp').controller("wechatApp.views.bindwechat", ["$scope","Page", function ($scope,page) {
        var vm = this;
        page.setTitle("绑定微信账号");
        vm.test = function() {
            alert("this is a demo! can alert");
        }
    }]);
})();