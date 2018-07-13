(function () {
    
    // page parametre site
    $(document).ready(function () {

        $('.dropdown-submenu > a').hover(function (e) {
            var submenu = $(this);

            if (submenu.is(':visible')) {

                $('.dropdown-submenu .dropdown-menu').hide();

                var subMenuToShow = submenu.next('.dropdown-menu');


                subMenuToShow.show();
            }
            e.stopPropagation();
        });


        $('.dropdown').on("hidden.bs.dropdown", function () {
            // hide any open menus when parent closes
            $('.dropdown-submenu .dropdown-menu').hide();
        });

    });
})()
