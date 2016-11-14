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
            $urlRouterProvider.otherwise('/bind-wechat');

            $stateProvider.state('account',
            {
                url: '/bind-wechat',
                templateUrl: '/App/Wechat/views/account/bind-wechat.cshtml',
                menu: 'Account'
            })
            .state('bind-email',
                    {
                        url: '/bind-email',
                        templateUrl: '/App/Wechat/views/account/bind-email.cshtml',
                        menu: 'Account'
                    })
            .state('modify-pwd',
                    {
                        url: '/modify-pwd',
                        templateUrl: '/App/Wechat/views/account/modifypwd.cshtml',
                        menu: 'Account'
                    })
            ;
        }]);
})()