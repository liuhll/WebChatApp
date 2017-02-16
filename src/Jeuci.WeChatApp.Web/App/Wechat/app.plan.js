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
            .state('caimeng',
	                {
	                    url: '/caimeng?isNeedCallBack&openId',
	                    templateUrl: '/App/Wechat/views/purchase/caimeng.cshtml',
	                    params: {
	                        isNeedCallBack: true,
	                        openId: ""
	                    }
	                })
            .state('serviceList',
	                {
	                    url: '/caimeng/:sid?openId',
	                    templateUrl: '/App/Wechat/views/purchase/serviceList.cshtml'

	                })
            .state('unifiedOrder',
	                {
	                    url: '/unifiedorder/?orderId&sid&serviceName&prepayId&orderPrice&description&openId',
	                    templateUrl: '/App/Wechat/views/purchase/unifiedOrder.cshtml',
                        params: {
                            orderId: "",
                            sid:"",
                            serviceName: "",
                            prepayId: "",
                            orderPrice: "0.00",
                            description: "",
                            openId: ""

                        }
	                }).state('purchaseMsg',
	                {
	                    url: '/purchasemsg/?code&msg&openId',
	                    templateUrl: '/App/Wechat/views/purchase/purchaseMsg.cshtml',
	                    params: {
	                        result: '',
	                        msg: '',
                            openId:""
	                    }
	                })
        	;
        }]);
})()