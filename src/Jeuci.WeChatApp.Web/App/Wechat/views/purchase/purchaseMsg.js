(function() {
    angular.module('planApp').controller('wechatApp.views.purchasemsg', ["$state", "$window",
        function ($state, $window) {
            var vm = this;

            vm.MsgInfo = $state.params;

            vm.backHome = function () {
                var hosturl = "http://www.camew.com/";
                $window.location.href = hosturl;
            }
        }
    ]);
})();