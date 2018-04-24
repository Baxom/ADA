﻿(function () {

    // page parametre site
    $(document).ready(function () {

        $('input.commune').autocomplete({
            delay: 300,
            source: function (request, response) {

                $.ajax({
                    dataType: "json",
                    url: "/reference/commune/auto-complete/" + request.term,
                }).done(function (result) {



                    response(result.data.map(function (commune) {
                        return {
                            label: commune.nom + '(' + commune.codePostal + ')',
                            value: commune.nom
                        }
                    }));


                });

            }
        });

        $('input.lieu').autocomplete({
            delay: 300,
            source: function (request, response) {

                $.ajax({
                    dataType: "json",
                    url: "/lieu/autocomplete?typeLieuFonctionnelId=" + $('input.lieu').data("type-lieu-fonctionnel")  + "&filtre=" + request.term
                }).done(function (result) {

                    response(result.map(function (lieu) {
                        return {
                            label: lieu.nom,
                            value: lieu.nom
                        }
                    }));


                });

            }
        });
    });
})()