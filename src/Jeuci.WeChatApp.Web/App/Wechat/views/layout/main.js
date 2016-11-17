(function () {
    angular.module('wechatApp').controller('wechatApp.views.main',
        ['$scope', 'Page', "Valid",function ($scope, page,valid) {
            $scope.Page = page;
            $scope.valid = valid;
        }]);
})();