(function () {

    // page parametre site
    $(document).ready(function () {

        $('[name="pagination"]').change(function () {
            
            var self = $(this);

            var form = self.closest('form');

            if (form) form.submit();
            
        });
    });
})()