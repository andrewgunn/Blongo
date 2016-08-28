"use strict";

(function($) {
    // Close the navigation bar when a link is clicked.
    $(document)
        .on("click",
            ".navbar",
            function(event) {
                var $target = $(event.target);

                if ($target.is("a") && $target.not(".navbar-toggler")) {
                    $("#navbar-collapsible").collapse("hide");
                }
                $target;
            });

    // Close the navigation bar when something anything else is clicked.
    $(document)
        .on("click",
            function(event) {
                var $target = $(event.target);
                var $navbarCollapsible = $("#navbar-collapsible");
                var isOpen = $navbarCollapsible.hasClass("in");

                if (isOpen && !$target.parents(".navbar").length) {
                    $navbarCollapsible.collapse("hide");
                }
            });
})(window.jQuery);