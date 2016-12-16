(function() {
    angular.module("planApp").controller('planApp.views.free',
        ["$state", "Error", "abp.services.app.lottery", function ($state, error, lottery) {
        var vm = this;

        lottery.getServiceList(null,{
            beforeSend: function () {
                $.showLoading("正在加载...");
            },
            complete: function () {
                $.hideLoading();
            }
        }).success(function (result) {
            if (result.code === 200) {
                vm.cpTypeList = result.data;
            } else {
                error.errorInfo = result.msg;
                window.location.href = window.location.origin + "/wechat/account/#/error";
            }
        });

        vm.getFreePlanList = function (sid) {
            $state.go("details", { sid: sid });
          
        }
    }]);
})();