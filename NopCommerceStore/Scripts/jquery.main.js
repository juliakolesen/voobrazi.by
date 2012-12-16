
jQuery(document).ready(function () {


    /* слайдер цен */
    var curSearch = document.location.search.substr(1);
    var maxValue = jQuery("input#maxCostConst").val();
    var curMax = getParam('maxCost');
    var curMin = getParam('minCost');
    if (curMax == 0) curMax = maxValue;
    jQuery("input#minCost").val(curMin);
    jQuery("input#maxCost").val(curMax);

    jQuery("#slider").slider({
        min: 0,
        max: maxValue,
        values: [0, maxValue],
        range: true,
        stop: function (event, ui) {
            jQuery("input#minCost").val(jQuery("#slider").slider("values", 0));
            jQuery("input#maxCost").val(jQuery("#slider").slider("values", 1));
            reloadPage();
        },
        slide: function (event, ui) {
            jQuery("input#minCost").val(jQuery("#slider").slider("values", 0));
            jQuery("input#maxCost").val(jQuery("#slider").slider("values", 1));
        }
    });

    jQuery("#slider").slider("values", 0, curMin);
    jQuery("#slider").slider("values", 1, curMax);

    jQuery("input#minCost").change(function () {
        changeMin();
    });

    function changeMin() {
        var value1 = jQuery("input#minCost").val();
        var value2 = jQuery("input#maxCost").val();

        if (parseInt(value1) > parseInt(value2)) {
            value1 = value2;
            jQuery("input#minCost").val(value1);
        }
        jQuery("#slider").slider("values", 0, value1);
    }

    jQuery("input#maxCost").change(function () {
        changeMax();
    });

    function changeMax() {
        var value1 = jQuery("input#minCost").val();
        var value2 = jQuery("input#maxCost").val();

        if (parseInt(value2) > parseInt(maxValue)) {
            value2 = maxValue;
            jQuery("input#maxCost").val(maxValue);
        }

        if (parseInt(value1) > parseInt(value2)) {
            value2 = value1;
            jQuery("input#maxCost").val(value2);
        }
        jQuery("#slider").slider("values", 1, value2);
    }

    // фильтрация ввода в поля
    jQuery("input#maxCost").keypress(function (event) {
        var key, keyChar;
        if (!event) event = window.event;

        if (event.keyCode) key = event.keyCode;
        else if (event.which) key = event.which;

        if (key == 13) {
            changeMax();
            return false;
        }

        if (key == null || key == 0 || key == 8 || key == 9 || key == 46 || key == 37 || key == 39) {
            return true;
        }
        keyChar = String.fromCharCode(key);

        if (!/\d/.test(keyChar)) return false;

        return true;
    });

    jQuery("input#minCost").keypress(function (event) {
        var key, keyChar;
        if (!event) event = window.event;

        if (event.keyCode) key = event.keyCode;
        else if (event.which) key = event.which;

        if (key == 13) {
            changeMin();
            reloadPage();
            return false;
        }

        if (key == null || key == 0 || key == 8 || key == 9 || key == 46 || key == 37 || key == 39) {
            return true;
        }
        keyChar = String.fromCharCode(key);

        if (!/\d/.test(keyChar)) return false;

        return true;
    });

    function reloadPage() {
        var minVal = jQuery("input#minCost").val();
        var maxVal = jQuery("input#maxCost").val();

        insertParam('minCost', minVal);
        insertParam('maxCost', maxVal);

        document.location.search = curSearch;
    }

    function insertParam(key, value) {
        key = escape(key); value = escape(value);

        if (curSearch.length != 0) {
            var kvp = curSearch.split('&');

            var i = kvp.length; var x; while (i--) {
                x = kvp[i].split('=');

                if (x[0] == key) {
                    x[1] = value;
                    kvp[i] = x.join('=');
                    break;
                }
            }

            if (i < 0) { kvp[kvp.length] = [key, value].join('='); }
            curSearch = kvp.join('&');
        } else {
            curSearch = key + "=" + value;
        }
    }

    function getParam(key) {
        key = escape(key);

        if (curSearch.length != 0) {
            var kvp = curSearch.split('&');

            var i = kvp.length;
            var x;
            while (i--) {
                x = kvp[i].split('=');

                if (x[0] == key) {
                    return parseInt(x[1]);
                }
            }
        }
        return 0;
    }
});
