(function () {

    // page parametre site
    $(document).ready(function () {

        $('[multi-select-api-checkbox-select-all]').click(function (e) {
            
            $('input[type="checkbox"][multi-select-api-button-checkbox]').prop('checked', true);
           
        });

        $('[multi-select-api-checkbox-deselect-all]').click(function (e) {

            $('input[type="checkbox"][multi-select-api-button-checkbox]').prop('checked', false);

            
        });

        // Fonctionnalité cocher/decocher tous
        $('[data-multi-select-api-button]').click(function () {

            var self = $(this);
            var data = self.data('multi-select-api-button');

            if (typeof data === 'string') data = data = JSON.parse(data); 

            var queryParam = $('input[type="checkbox"][multi-select-api-button-checkbox]:checked').toArray().map(function (elt) {
                return data.key + "=" + $(elt).data(data.checkboxKey);
                })

            var queryString = "";
            
            if (queryParam.length) {

                queryString += queryParam.join('&');

                if (data.extendParams) queryString += "&" + data.extendParams;

                window.open(data.api + "?" + queryString, "_blank")
            }

            

        });
    });
})()
