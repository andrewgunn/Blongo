'use strict';

(function ($, Markdown) {
    var markdownConverter = new Markdown.Converter();
    var markdownSanitizingConverter = Markdown.getSanitizingConverter();

    $(function () {
        var markdownEditorCount = $('[id^=wmd-input]').length;

        for (var i = 0; i < markdownEditorCount; i++) {
            var inputSelector = '#wmd-input';
            var previewSelector = '#wmd-preview';

            if (i > 0) {
                inputSelector += '-' + (i + 1);
                previewSelector += '-' + (i + 1);
            }

            var $input = $(inputSelector).addClass('m-b-0');
            var markdownEditor = new Markdown.Editor($input.data('sanitize') ? markdownSanitizingConverter : markdownConverter, i > 0 ? '-' + (i + 1) : null);
            var $markdownStylingLink = $('<a>').html('<small>Styling with Markdown is supported</small>')
                .prop('href', 'https://guides.github.com/features/mastering-markdown')
                .prop('target', '_new');
            var $markdownStylingContainer = $('<div>').addClass('m-b-1')
                .append($markdownStylingLink);
            $markdownStylingContainer.insertAfter($input);

            markdownEditor.run();
        }
    });
})(window.jQuery, window.Markdown);