"use strict";

(function() {
    $(document)
        .on("submit",
            "form",
            function(event) {
                var $form = $(event.currentTarget);

                if ($form.data("submitted")) {
                    event.preventDefault();

                    return;
                }

                $form.data("submitted", true);
            });
})();