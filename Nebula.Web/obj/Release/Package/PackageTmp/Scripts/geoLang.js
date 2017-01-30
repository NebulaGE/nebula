

jQuery.fn.extend({
    insertAtCaret: function (myValue) {
        return this.each(function (i) {
            if (document.selection) {
                //For browsers like Internet Explorer
                this.focus();
                sel = document.selection.createRange();
                sel.text = myValue;
                this.focus();
            }
            else if (this.selectionStart || this.selectionStart == '0') {
                //For browsers like Firefox and Webkit based
                var startPos = this.selectionStart;
                var endPos = this.selectionEnd;
                var scrollTop = this.scrollTop;
                this.value = this.value.substring(0, startPos) + myValue + this.value.substring(endPos, this.value.length);
                this.focus();
                this.selectionStart = startPos + myValue.length;
                this.selectionEnd = startPos + myValue.length;
                this.scrollTop = scrollTop;
            } else {
                this.value += myValue;
                this.focus();
            }
        });
    }
});

jQuery.fn.geoKeyboard = function (options) {
    var settings = $.extend({
        'enable': true, //this plugin is enabled by default
        'switcher': 'switcher' //the id of the switcher checkbox
    }, options),

		latin = 'abgdevzTiklmnopJrstufqRySCcZwWxjh',
		isEnabled = settings.enable,
		switcher = $('input#' + settings.switcher);

    //check/uncheck the checkbox if the plugin is enabled/disabled from settings (enabled by default)
    switcher.prop("checked", settings.enable);

    //enable/disable the plugin if the checkbox is checked/unchecked
    $(switcher).change(function () {
        isEnabled = switcher.prop('checked');
    });

    return this.find('input.geoKeyboard').each(function () {
        var $this = $(this);

        $this.keypress(function (e) {
            var code = e.keyCode ? e.keyCode : e.charCode,
				i = latin.indexOf(String.fromCharCode(code));

            //enabling/disabling by pressing the ~ key while typing
            if (code == 96) {
                isEnabled = !isEnabled;
                switcher.prop("checked", !switcher.prop("checked"));
                e.preventDefault();
            }

            if (!isEnabled) return;

            if (i > -1) {
                $this.insertAtCaret(String.fromCharCode(i + 4304));
                $("#switcher").focus();
                $(this).focus();
                e.preventDefault();
            }
        });
    });
};