(function () {
    angular.module('wechatApp').controller('wechatApp.views.main',
        ['$scope', 'Page',"Error", "Tips",function ($scope, page,error,tips) {
            $scope.Page = page;
            $scope.Tips = tips;
            $scope.Error = error;
        }]);
})();