(function($) {

    $(".checkbox").each(function() {
        var checkbox = $(this);
        var i = checkbox.siblings("i");
        checkbox.after(i);
    });

})(jQuery);

(function ($) {
    $(function () {


        function isPageEditor() {
            if (typeof Sitecore == "undefined") {
                return false;
            }
            if (typeof Sitecore.PageModes == "undefined" || Sitecore.PageModes == null) {
                return false;
            }
            return Sitecore.PageModes.PageEditor != null;
        }

        if (isPageEditor() == false) {

            $("pre").each(function () {

                var $pre = $(this);
                $pre.addClass("prettyprint");
            });

        }

        $("[scfieldtype='checkbox']").each(function () {
            var field = $(this);
            var input = $(this).find("input");

            var id = input.attr("data-id");
            var lang = input.attr("data-lang");
            var ver = input.attr("data-ver");
            var rev = input.attr("data-rev");
            var fld = input.attr("data-fld");
            var isChecked = input.attr("checked") == "checked";

            var checkboxed = $("<input type='checkbox' checked='checked'>");
            var unCheckboxed = $("<input type='checkbox'>");

            var itemUri = new Sitecore.ItemUri(id, lang, ver, rev);

            function checked() {

                checkboxed.css("display", "inline");
                unCheckboxed.css("display", "none");
                Sitecore.WebEdit.setFieldValue(itemUri, fld, "1");
            }
            function unchecked() {
                checkboxed.css("display", "none");
                unCheckboxed.css("display", "inline");
                Sitecore.WebEdit.setFieldValue(itemUri, fld, "0");

            }

            if (isChecked) {
                checked();
            } else {
                unchecked();
            }

            checkboxed.click(unchecked);
            unCheckboxed.click(checked);

            field.after(checkboxed);
            field.after(unCheckboxed);
            field.css("display", "none");
        });




        $(".input-validation-error").parent().addClass("error-state");
    });
})(jQuery);

var ParallaxSlider = function () {

    return {

        //Parallax Slider
        initParallaxSlider: function () {
            $('#da-slider').cslider({
                current: 0,
                // index of current slide

                bgincrement: 0,
                // increment the background position 
                // (parallax effect) when sliding

                autoplay: true,
                // slideshow on / off

                interval: 10000
                // time between transitions
            });
        },

    };

}();

jQuery(document).ready(function () {
    ParallaxSlider.initParallaxSlider();
});