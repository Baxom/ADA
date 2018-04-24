(function () {

    // page parametre site
    $(document).ready(function () {

        // Fonctionnalité cocher/decocher tous
        $('#genere-pdf').click(function () {
            
            var queryParam = $('input[type="checkbox"][name="selection-pretre"]:checked').toArray().map(function (elt) {
                    return "ids=" + $(elt).data('id');
                })
              
            if (queryParam.length) {
                window.open("/pretre/fiches?" + queryParam.join('&'), "_blank")
            }
        });
    });
})()