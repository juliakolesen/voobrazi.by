jQuery(document).ready(function () {
    var curSearch = document.location.search.substr(1);

    var maxHeigth = jQuery("input#maxHeightConst").val();
    var maxWidth = jQuery("input#maxWidthConst").val();

    var curMaxHeight = getParam('maxHeight');
    var curMinHeight = getParam('minHeight');
    if (curMaxHeight == 0) curMaxHeight = maxHeigth;
    jQuery("input#minHeight").val(curMinHeight);
    jQuery("input#maxHeight").val(curMaxHeight);

    var curMaxWidth = getParam('maxWidth');
    var curMinWidth = getParam('minWidth');
    if (curMaxWidth == 0) curMaxWidth = maxWidth;
    jQuery("input#minWidth").val(curMinWidth);
    jQuery("input#maxWidth").val(curMaxWidth);

    $(function () {

        $('#minHeight').change(function () {
            changeMinHeight();
        });

        $('#maxHeight').change(function () {
            changeMaxHeight();
        });

        $("#slider_height").slider({
            range: true,
            orientation: "vertical",
            min: 0,
            max: maxHeigth,
            values: [0, maxHeigth],
            slide: function (event, ui) {
                $('#minHeight').val(ui.values[0]);
                $('#maxHeight').val(ui.values[1]);
            },
            stop: function (event, ui) {
                jQuery("input#minHeight").val(jQuery("#slider_height").slider("values", 0));
                jQuery("input#maxHeight").val(jQuery("#slider_height").slider("values", 1));
                reloadPage();
            }
        });

        $('#slider_height').slider("values", 0, curMinHeight);
        $('#slider_height').slider("values", 1, curMaxHeight);
        $('#slider_height').slider({ colors: { primary: '#FF0000', secondary: '#0000FF'} });
    });


    $(function () {
        $('#minWidth').change(function () {
            changeMinWidth();
        });

        $('#maxWidth').change(function () {
            changeMaxWidth();
        });

        $("#slider_width").slider({
            range: true,
            min: 0,
            max: maxWidth,
            values: [0, maxWidth],
            slide: function (event, ui) {
                $('#minWidth').val(ui.values[0]);
                $('#maxWidth').val(ui.values[1]);
            },
            stop: function (event, ui) {
                jQuery("input#minWidth").val(jQuery("#slider_width").slider("values", 0));
                jQuery("input#maxWidth").val(jQuery("#slider_width").slider("values", 1));
                reloadPage();
            }
        });

        $('#slider_width').slider("values", 0, curMinWidth);
        $('#slider_width').slider("values", 1, curMaxWidth);

    });

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

    function reloadPage() {
        var minH = jQuery("input#minHeight").val();
        var maxH = jQuery("input#maxHeight").val();
        var minW = jQuery("input#minWidth").val();
        var maxW = jQuery("input#maxWidth").val();

        insertParam('minHeight', minH);
        insertParam('maxHeight', maxH);
        insertParam('minWidth', minW);
        insertParam('maxWidth', maxW);

        document.location.search = curSearch;
    }

    function insertParam(key, value) {
        key = escape(key);
        value = escape(value);

        if (curSearch.length != 0) {
            var kvp = curSearch.split('&');

            var i = kvp.length;
            var x;
            while (i--) {
                x = kvp[i].split('=');

                if (x[0] == key) {
                    x[1] = value;
                    kvp[i] = x.join('=');
                    break;
                }
            }

            if (i < 0) {
                kvp[kvp.length] = [key, value].join('=');
            }
            curSearch = kvp.join('&');
        } else {
            curSearch = key + "=" + value;
        }
    }

    // фильтрация ввода в поля
    jQuery("input#maxHeight").keypress(function (event) {
        var key, keyChar;
        if (!event) event = window.event;

        if (event.keyCode) key = event.keyCode;
        else if (event.which) key = event.which;

        if (key == 13) {
            changeMaxHeight();
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

    jQuery("input#minHeight").keypress(function (event) {
        var key, keyChar;
        if (!event) event = window.event;

        if (event.keyCode) key = event.keyCode;
        else if (event.which) key = event.which;

        if (key == 13) {
            changeMinHeight();
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

    function changeMinHeight() {
        var value1 = jQuery("input#minHeight").val();
        var value2 = jQuery("input#maxHeight").val();

        if (parseInt(value1) > parseInt(value2)) {
            value1 = value2;
            jQuery("input#minHeight").val(value1);
        }
        jQuery("#slider_height").slider("values", 0, value1);
        jQuery("#slider_height").slider("values", 1, value2);
    }

    function changeMaxHeight() {
        var value1 = jQuery("input#minHeight").val();
        var value2 = jQuery("input#maxHeight").val();

        if (parseInt(value2) > parseInt(maxHeigth)) {
            value2 = maxHeigth;
            jQuery("input#maxHeight").val(maxHeigth);
        }

        if (parseInt(value1) > parseInt(value2)) {
            value2 = value1;
            jQuery("input#maxHeight").val(value2);
        }

        jQuery("#slider_height").slider("values", 1, value2);
    }

    jQuery("input#maxWidth").keypress(function (event) {
        var key, keyChar;
        if (!event) event = window.event;

        if (event.keyCode) key = event.keyCode;
        else if (event.which) key = event.which;

        if (key == 13) {
            changeMaxWidth();
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

    jQuery("input#minWidth").keypress(function (event) {
        var key, keyChar;
        if (!event) event = window.event;

        if (event.keyCode) key = event.keyCode;
        else if (event.which) key = event.which;

        if (key == 13) {
            changeMinWidth();
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

    function changeMinWidth() {
        var value1 = jQuery("input#minWidth").val();
        var value2 = jQuery("input#maxWidth").val();

        if (parseInt(value1) > parseInt(value2)) {
            value1 = value2;
            jQuery("input#minWidth").val(value1);
        }
        jQuery("#slider_width").slider("values", 0, value1);
    }

    function changeMaxWidth() {
        var value1 = jQuery("input#minWidth").val();
        var value2 = jQuery("input#maxWidth").val();

        if (parseInt(value2) > parseInt(maxWidth)) {
            value2 = maxHeigth;
            jQuery("input#maxWidth").val(maxWidth);
        }

        if (parseInt(value1) > parseInt(value2)) {
            value2 = value1;
            jQuery("input#maxWidth").val(value2);
        }

        jQuery("#slider_width").slider("values", 1, value2);
    }
});
