var TipHepler = (function () {
    var tips = {};
    tips.ShowMsg = function() {
        var $tooltips = $('.js_tooltips');
        if ($tooltips.css('display') !== 'none') return;
        $('.page.cell').removeClass('slideIn');
        $tooltips.css('display', 'block');
        setTimeout(function () {
            $tooltips.css('display', 'none');
        }, 2000);
    };

    return tips;
})();