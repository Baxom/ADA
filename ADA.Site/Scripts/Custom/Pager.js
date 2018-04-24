(function () {

    // page parametre site
    $(document).ready(function () {

        $('ul.pagination li.page-item a.page-link').click(function () {
            var self = this;
            var classForm = self.className.split(' ').find(function (x) { return x.startsWith('form-') });

            if (classForm) {
                var formName = classForm.replace('form-', '');
                var form = $('form[name="' + formName + '"]');
                if (form) self.search += "&" + form.serialize();
            }
        });
    });
})()