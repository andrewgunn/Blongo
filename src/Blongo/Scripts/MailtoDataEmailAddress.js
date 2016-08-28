"use strict";

(function(document, window, $) {
    $(function() {
        $(document)
            .on("click",
                "[data-emailaddress]",
                function(event) {
                    event.preventDefault();

                    var emailAddress = $(this)
                        .data("emailaddress")
                        .replace(/,/g, "");

                    window.location = "mailto:" + emailAddress;
                });

        $(function() {
            $("[data-emailaddress]")
                .each(function() {
                    var $this = $(this);

                    var emailAddress = $this.data("emailaddress")
                        .replace(/,/g, "");

                    $this.prop("title", emailAddress);
                });
        });
    });
})(document, window, window.jQuery);