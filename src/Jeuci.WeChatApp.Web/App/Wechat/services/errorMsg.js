(function() {
    angular.module("wechatApp").factory("Error", function () {
        var title = "错误";
        return {
            title: title,
            errorInfo:"",
            setErrorInfo : function(errorMsg) {
                this.errorInfo = errorMsg;
            }
        };
    });

    angular.module("planApp").factory("Error", function () {
        var title = "错误";
        return {
            title: title,
            errorInfo: "",
            setErrorInfo: function (errorMsg) {
                this.errorInfo = errorMsg;
            }
        };
    });
})();