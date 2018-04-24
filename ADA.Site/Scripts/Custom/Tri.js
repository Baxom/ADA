(function () {

    // page parametre site
    $(document).ready(function () {

        $('[name="tri-select"]').change(function () {
            
            var self = $(this);

            var form = self.closest('form');

            if (form) form.submit();
            
        });
    });
})()