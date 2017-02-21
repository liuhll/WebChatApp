(function () {
    angular.module('planApp').controller('planApp.views.unfiedOrder',
        ['$state', 'abp.services.app.purchase', 'abp.services.app.recharge',
        function ($state, purchase, recharge) {
            var vm = this;
            
            vm.serviceInfo = $state.params;
            debugger;
            vm.purchase = function (goodtype) {              
                purchase.generatePayOrder({
                    id: vm.serviceInfo.orderId,
                    openId: vm.serviceInfo.openId,
                    cost: vm.serviceInfo.orderPrice,
                    goodsInfo: vm.serviceInfo.description,
                    goodsName: vm.serviceInfo.serviceName,
                    goodsId: vm.serviceInfo.sid,
                    goodType:goodtype
                }).success(function (result1) {

                    if (result1.code === 200) {
                        var nonceStr = randomString(32);
                        //参与签名的参数有appId, timeStamp, nonceStr, package, signType。
                        purchase.getPaySign('prepay_id=' + vm.serviceInfo.prepayId, nonceStr).success(function (result) {
                            wx.chooseWXPay({
                                timestamp: result.timestamp, // 支付签名时间戳，注意微信jssdk中的所有使用timestamp字段均为小写。但最新版的支付后台生成签名使用的timeStamp字段名需大写其中的S字符
                                nonceStr: result.nonceStr, // 支付签名随机串，不长于 32 位
                                package: result.package, // 统一支付接口返回的prepay_id参数值，提交格式如：prepay_id=***）
                                signType: 'MD5', // 签名方式，默认为'SHA1'，使用新版支付需传入'MD5'
                                paySign: result.paySign, // 支付签名
                                success: function (res) {
                                    // 支付成功后的回调函数     
                                    if (goodtype === 1) {
                                      recharge.clientCompleteServiceOrder(result1.data)
                                      .success(function (result2) {
                                          $state.go("purchaseMsg",
                                               {
                                                   code: result2.msg,
                                                   msg: result2.data,
                                                   openId: vm.serviceInfo.openId
                                               });
                                      });
                                    } else {
                                      purchase.clientCompleteServiceOrder(result1.data)
                                      .success(function (result2) {
                                          $state.go("purchaseMsg",
                                               {
                                                   code: result2.msg,
                                                   msg: result2.data,
                                                   openId: vm.serviceInfo.openId
                                               });
                                      });
                                    }                                                                                                                                                             
                                },
                                fail: function (res) {
                                    $state.go("purchaseMsg",
                                                {
                                                    code: "FAIL",
                                                    msg: "付款失败，请与我们联系"
                                                });
                                }
                            });
                        });

                    } else {
                        alert(result1.msg);
                    }                   
                });
              
            }

        }]);



    function randomString(length) {
        var str = '';
        for (; str.length < length; str += Math.random().toString(36).substr(2));
        return str.substr(0, length);
    }
})();