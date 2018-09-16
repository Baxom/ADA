(function () {
    
    // page parametre site
    $(document).ready(function () {

        var contextElement = $('.markable-context');

        contextElement.each(function (ind, elt) {
            var instance = new Mark(elt);

            var keyWords = contextElement.data('mark-keyword');

            $.each(keyWords, function (indKeyword, keyWord) {
                instance.mark(keyWord.val, {
                    element: 'span',
                    className: 'highlighted-text',
                    accuracy: keyWord.wordBoundary ? 'exactly' : 'partially'
                });
            }
            );

            
        });

      

    });
})()
