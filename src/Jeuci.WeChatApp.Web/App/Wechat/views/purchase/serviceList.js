(function () {
    angular.module('planApp').controller('planApp.views.serviceList', ['$state', 'abp.services.app.lottery',
        function ($state, lottery) {
            var vm = this;
            var sid = $state.params.sid,
                openId = $state.params.openId;
            vm.ServicePriceList = lottery.serverPriceList(sid, openId, {
                beforeSend: function () {
                    $.showLoading("正在加载...");
                },
                complete: function () {
                    $.hideLoading();
                }
            }).success(function (result) {
                if (result.code === 200) {
                    vm.showServiceList = true;
                  
                    vm.servicePrice = result.data;

                } else {
                    vm.showServiceList = false;
                    vm.msg = result.msg;
                }
            });
        }]);
})();