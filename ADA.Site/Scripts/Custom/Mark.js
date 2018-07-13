(function () {
    
    // page parametre site
    $(document).ready(function () {

        var contextElement = $('.markable-context');

        contextElement.each(function (ind, elt) {
            var instance = new Mark(elt);
            instance.mark(contextElement.data('mark-keyword'), {
                element: 'span',
                className: 'highlighted-text'
            });
        });

      

    });
})()
