(function () {
    angular.module('wechatApp').controller('wechatApp.views.main',
        ['$scope', 'Page', function ($scope, page) {
            $scope.Page = page;
        }]);
})();