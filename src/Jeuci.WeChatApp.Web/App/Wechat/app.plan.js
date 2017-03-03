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
	                    url: '/?orderId&sid&serviceName&prepayId&orderPrice&description&openId&priceId',
	                    templateUrl: '/App/Wechat/views/purchase/unifiedOrder.cshtml',
                        params: {
                            orderId: "",
                            sid:"",
                            serviceName: "",
                            prepayId: "",
                            orderPrice: "0.00",
                            description: "",
                            openId: "",
                            priceId:""

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
            .state('recharge',
	                {
	                    url: '/freelist?isNeedCallBack&openId',
	                    templateUrl: '/App/Wechat/views/recharge/freelist.cshtml',
	                    params: {
	                        isNeedCallBack: true,
	                        openId: ""
	                    }
	                })
                .state('serviceListByUid',
	                {
	                    url: '/serviceList/:sid?uid',
	                    templateUrl: '/App/Wechat/views/purchase/serviceList.cshtml'
	                })
            .state('alipayUnifiedOrder',
	                {
	                    url: '/alipay/?orderId&sid&serviceName&orderPrice&description&uid&userName&priceId',
	                    templateUrl: '/App/Wechat/views/purchase/unifiedOrder.cshtml',
	                    params: {
	                        orderId: "",
	                        sid: "",
	                        serviceName: "",
	                        orderPrice: "0.00",
	                        description: "",
	                        uid: "",
	                        userName: "",
	                        priceId:""

	                    }
	                })
        	;
        }]);
})()