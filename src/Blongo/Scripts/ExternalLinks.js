"use strict";

(function($) {
    var hostRegex = new RegExp("/" + window.location.host + "/");

    $(document)
        .on("click",
            "a",
            function(event) {
                if (!hostRegex.test(this.href)) {
                    event.preventDefault();
                    event.stopPropagation();

                    window.open(this.href, "_blank");
                }
            });
})(window.jQuery);