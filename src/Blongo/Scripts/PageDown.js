'use strict';

(function ($, Markdown, highlight) {
    var markdownConverter = new Markdown.Converter();

    $(function () {
        var markdownEditorCount = $('[id^=wmd-input]').length;

        for (var i = 0; i < markdownEditorCount; i++) {
            var markdownEditor = new Markdown.Editor(markdownConverter, i > 0 ? '-' + (i + 1) : null);
            var inputSelector = '#wmd-input';
            var previewSelector = '#wmd-preview';

            if (i > 0) {
                inputSelector += '-' + (i + 1);
                previewSelector += '-' + (i + 1);
            }

            var $inputSelector = $(inputSelector).addClass('m-b-0');
            var $markdownStylingLink = $('<a>').html('<small>Styling with Markdown is supported</small>')
                .prop('href', 'https://guides.github.com/features/mastering-markdown')
                .prop('target', '_new');
            var $markdownStylingContainer = $('<div>').addClass('m-b-1')
                .append($markdownStylingLink);
            $markdownStylingContainer.insertAfter($inputSelector);

            markdownEditor.run();
        }
    });
})(window.jQuery, window.Markdown, window.hljs);