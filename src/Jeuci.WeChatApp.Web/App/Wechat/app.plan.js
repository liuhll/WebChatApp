(function () {
	'use strict';

	var wechatApp = angular.module('planApp', [
        'ngAnimate',
        'ngSanitize',

        'ui.router',
        'ui.bootstrap',
        'ui.jq',

        'abp'
	]);

	wechatApp.config(['$stateProvider', '$urlRouterProvider', '$locationProvider',
        function ($stateProvider, $urlRouterProvider) {
        	$urlRouterProvider.otherwise('/free');

        	$stateProvider.state('free',
            {
            	url: '/free',
            	templateUrl: '/App/Wechat/views/plan/freeIndex.cshtml'
            })
            .state('details',
                    {
                    	url: '/details/:sid',
                    	templateUrl: '/App/Wechat/views/plan/freeDetails.cshtml'
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