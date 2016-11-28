var BoolHelper = (function (helper) {
    var boolHelper = helper;
    boolHelper.parseBool = function (str) {
        if (str == null)
            return false;

        if (typeof str === 'boolean') {
            return (str === true);
        }

        if (typeof str === 'string') {
            if (str == "")
                return false;

            str = str.replace(/^\s+|\s+$/g, '');
            if (str.toLowerCase() == 'true' || str.toLowerCase() == 'yes')
                return true;

            str = str.replace(/,/g, '.');
            str = str.replace(/^\s*\-\s*/g, '-');
        }

        // var isNum = string.match(/^[0-9]+$/) != null;
        // var isNum = /^\d+$/.test(str);
        if (!isNaN(str))
            return (parseFloat(str) != 0);

        return false;
    };

    return boolHelper;

})(window.BoolHelper || {});