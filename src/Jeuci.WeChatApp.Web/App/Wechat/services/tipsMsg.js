(function() {
    angular.module('wechatApp').factory('Tips', function() {
        return {
            msg: null,
           // isSuccess: false,
            isError:false
    }
    });

    angular.module('planApp').factory('Tips', function () {
        return {
            msg: null,
            // isSuccess: false,
            isError: false
        }
    });
})();