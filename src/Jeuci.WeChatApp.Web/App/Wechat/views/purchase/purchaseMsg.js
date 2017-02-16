(function() {
    angular.module('planApp').controller('wechatApp.views.purchasemsg', ["$state", "$window",
        function ($state, $window) {
            var vm = this;

            vm.MsgInfo = $state.params;

            vm.backHome = function () {
                var host = $window.location.origin;
                var hosturl = host + "/purchase/#/caimeng?isNeedCallBack=false&openId=" + vm.MsgInfo.openId;
                $window.location.href = hosturl;
            }
        }
    ]);
})();