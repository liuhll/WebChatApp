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

    wechatApp.config(['$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            $urlRouterProvider.otherwise('/bindwechat');

            $stateProvider.state('bindwechat',
            {
                url: '/bindwechat',
                templateUrl: '/App/Wechat/views/account/bindwechat.cshtml'
            })
            .state('bindemail',
                    {
                        url: '/bindemail',
                        templateUrl: '/App/Wechat/views/account/bindemail.cshtml'                  
                    })
            .state('modifypwd',
                    {
                        url: '/modifypwd',
                        templateUrl: '/App/Wechat/views/account/modifypwd.cshtml'
                    })
            ;
        }]);
})()