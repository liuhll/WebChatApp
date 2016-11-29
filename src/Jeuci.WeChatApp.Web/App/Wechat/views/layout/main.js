(function () {
    angular.module('wechatApp').controller('wechatApp.views.main',
        ['$scope', 'Page',"Error", "Valid",function ($scope, page,error,valid) {
            $scope.Page = page;
            $scope.valid = valid;
            $scope.Error = error;
        }]);
})();