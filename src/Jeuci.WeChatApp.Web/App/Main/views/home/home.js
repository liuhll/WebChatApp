(function() {
    var controllerId = 'app.views.home';
    angular.module('app').controller(controllerId, [
        '$scope',
        'abp.services.app.wechatMenu',
        function ($scope, wechatMenuService) {
            var vm = this;
            //Home logic...       
            vm.creatWechatMenu = function () {
                abp.message.confirm("你确定要更新菜单？非必要，不建议使用，一般用于开发环境!",
                    "警告",
                    function (iscomfired) {
                        if (iscomfired) {
                            abp.ui.setBusy();
                            wechatMenuService.createWechatMenu().success(function (result) {
                                // alert(result.msg);
                                if (result.code === 200) {
                                    abp.message.success(result.msg + "微信公共号菜单创建/更新成功", "success");
                                } else {
                                    abp.message.error(result.msg, "fail");
                                }
                            }).finally(function () {
                                abp.ui.clearBusy();
                            });
                        }
                      
                    });
              
              
            };
        }
    ]);
})();