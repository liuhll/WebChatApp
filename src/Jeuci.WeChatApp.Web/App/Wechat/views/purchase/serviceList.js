(function () {
    angular.module('planApp').controller('planApp.views.serviceList', ['$scope','$state', 'abp.services.app.lottery', 'abp.services.app.purchase',
        function ($scope,$state, lottery, purchase) {
            var vm = this;
            vm.sid = $state.params.sid;
            vm.openId = $state.params.openId;
            vm.uid = $state.params.uid;
            if (vm.openId) {
                vm.ServicePriceList = lottery.serverPriceList(vm.sid, vm.openId, {
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
                vm.purchase = function () {

                    if (vm.purchaseServiceInfo == null ||
                        vm.purchaseServiceInfo == undefined ||
                        vm.purchaseServiceInfo.purchaseServiceInfo === {}) {
                        alert("请选择您要购买的授权服务");
                        return;
                    }

                    //1. 生成商户订单
                    //2. $state
                    purchase.getUnifiedOrder({
                        total_fee: vm.purchaseServiceInfo.price,
                        body: vm.purchaseServiceInfo.authDesc,
                        openid: vm.openId,
                        description: vm.purchaseServiceInfo.description,
                        sid: vm.purchaseServiceInfo.id
                    }, {
                        beforesend: function () {
                            $.showLoading("正在生成订单，请稍等...");
                        },
                        complete: function () {
                            $.hideLoading();
                        }
                    }).success(function (result) {
                        if (result.code === 200) {
                            $state.go("unifiedOrder", result.data);
                        } else {
                            alert(result.msg);
                        }
                    }).error(function (result) {
                        alert("订单生成失败，请稍后重试!");
                    });

                }
            }
           
            if (vm.uid) {
                vm.ServicePriceList = lottery.serverPriceListByUid(vm.sid, vm.uid, {
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

                vm.purchase = function() {
                    if (vm.purchaseServiceInfo == null ||
                         vm.purchaseServiceInfo == undefined ||
                         vm.purchaseServiceInfo.purchaseServiceInfo === {}) {
                        alert("请选择您要购买的授权服务");
                        return;
                    }

                    //1. 生成商户订单
                    //2. $state
                    purchase.getAlipayUnifiedOrder({
                        total_fee: vm.purchaseServiceInfo.price,
                        body: vm.purchaseServiceInfo.authDesc,
                        uid: vm.uid,
                        description: vm.purchaseServiceInfo.description,
                        sid: vm.purchaseServiceInfo.id,
                        userName: vm.servicePrice.userName
                    }, {
                        beforesend: function () {
                            $.showLoading("正在生成订单，请稍等...");
                        },
                        complete: function () {
                            $.hideLoading();
                        }
                    }).success(function (result) {                       
                        if (result.code === 200) {
                            debugger;
                            $state.go("alipayUnifiedOrder", result.data);
                        } else {
                            alert(result.msg);
                        }
                    }).error(function (result) {
                        alert("订单生成失败，请稍后重试!");
                    });
                
                }
            }
        }]);
})();