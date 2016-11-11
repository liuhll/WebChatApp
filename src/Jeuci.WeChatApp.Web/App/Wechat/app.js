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
            $urlRouterProvider.otherwise('/index');

            $stateProvider.state('account',
            {
                url: '/index',
                templateUrl: '/App/Wechat/views/account/index.cshtml',
                menu: 'Account'
            })
            .state('bindemail',
                    {
                        url: '/bindemail',
                        templateUrl: '/App/Wechat/views/account/bindemail.cshtml',
                        menu: 'Bindemail'
                    })
            .state('modifypwd',
                    {
                        url: '/modifypwd',
                        templateUrl: '/App/Wechat/views/account/modifypwd.cshtml',
                        menu: 'Modifypwd'
                    })
            ;
        }]);
})()