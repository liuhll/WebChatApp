(function () {
    'use strict';

    var wechatApp = angular.module('wechatApp', [
        'ngAnimate',
        'ngSanitize',

        'ui.router',
        'ui.bootstrap',
        'ui.jq',

        'abp'
    ]);

    wechatApp.config(['$stateProvider', '$urlRouterProvider','$locationProvider',
        function ($stateProvider, $urlRouterProvider, $locationProvider) {
            $urlRouterProvider.otherwise('/bindwechat');

            $stateProvider.state('bindwechat',
            {
                url: '/bindwechat',
                templateUrl: '/App/Wechat/views/account/bindwechat.cshtml'
            })
            .state('wechatForjeuci',
                    {
                        url: '/wechatforjeuci?isNeedCallBack&openId',
                        templateUrl: '/App/Wechat/views/account/wechatForjeuci.cshtml',
                        params: {
                            isNeedCallBack: true,
                            openId:""
                        }
                    })
            .state('bindemail',
                    {
                        url: '/bindemail/:openId',
                        templateUrl: '/App/Wechat/views/account/bindemail.cshtml'
                    })
            .state('modifypwd',
                    {
                        url: '/modifypwd',
                        templateUrl: '/App/Wechat/views/password/modifypwd.cshtml'
                    })
            .state('retrievepwd',
                    {
                        url: '/retrievepwd',
                        templateUrl: '/App/Wechat/views/password/retrievepwd.cshtml'
                    })
            .state('error',
                    {
                        url: '/error',
                        templateUrl: '/App/Wechat/views/error/errorPage.cshtml'
                    })
            ;
           
            //$locationProvider.html5Mode({
            //    enabled: true,
            //    requireBase: false
            //});
        }]);
})()