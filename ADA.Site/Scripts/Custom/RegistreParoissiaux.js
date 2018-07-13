(function () {

    $(document).ready(function () {

        $('#TypeActe').change(function (elt) {
            var self = $(this);

            $("[data-filtre-acte-value]").addClass("d-none");
            $("[data-filtre-acte-value] input").val(null);
            if (self.val()) {
                $("[data-filtre-acte-value='" + self.val() + "']").removeClass("d-none");
            }

        });
    });
})()
