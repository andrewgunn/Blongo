'use strict';

(function ($, undefined) {
    $(document).on('submit', '.form-confirm', function (event) {
        var result = confirm("Are you sure?");

        if (!result) {
            event.preventDefault();
        }
    });
})(window.jQuery);