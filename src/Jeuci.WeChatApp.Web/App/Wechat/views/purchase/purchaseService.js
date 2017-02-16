
// var timestamp = Math.round(new Date().getTime() / 1000),
var nonceStr = RandomString(32);
var url = getOriginUrl();
abp.services.app.purchase.getWxpayOptions(nonceStr, url).done(function (options) {
    wx.config({
        debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
        appId: options.appId, // 必填，
        timestamp: options.timestamp, // 必填，生成签名的时间戳
        nonceStr: options.nonceStr, // 必填，生成签名的随机串
        signature: options.signature,// 必填，签名，见附录1
        jsApiList: ['chooseWXPay'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
    });
    wx.ready(function () {
    });

    wx.error(function (res) {

        // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，也可以在返回的res参数中查看，对于SPA可以在这里更新签名。
        // alert("支付接口异常，无法进行支付，请与我们联系！");
        alert(res.err_msg);

    });

});


function getOriginUrl() {
    var url = location.href;
    if (location.hash.length) {
        url = url.substr(0, url.indexOf(location.hash));
    }
    return url;
}


function RandomString(length) {
    var str = '';
    for (; str.length < length; str += Math.random().toString(36).substr(2));
    return str.substr(0, length);
}

