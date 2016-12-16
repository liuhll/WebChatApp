(function() {
    angular.module("planApp").controller('planApp.views.freeDetails',
        ["$state", "Error", "abp.services.app.lottery",
            function ($state, error, lottery) {
            var vm = this;
                var sid = $state.params.sid;    
        lottery.planList(sid,
        {
            beforeSend: function() {
                $.showLoading("正在加载...");
            },
            complete: function() {
                $.hideLoading();
            }
        }).success(function (result) {
            if (result.code === 200) {
                vm.planList = result.data;
            } else {
                error.errorInfo = result.msg;
                window.location.href = window.location.origin + "/wechat/plan/#/error";
            }
        });
    }]);
})();