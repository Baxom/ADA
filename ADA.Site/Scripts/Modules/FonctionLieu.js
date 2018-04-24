(function () {

    var jqHtmlModuleDiv = $("#recherche-lieu-fonction")[0];

    function FonctionLieuViewModel() {
        var self = this;

        
        self.typeLieuChanged = function () {

            var typeRecherche = self.fonctionLieuViewModel.typesLieu().find(function (x) { return x.id() == self.typeLieuId() }).typeRecherche();


            if (typeRecherche == 2) {
                getLieu(self.typeLieuId())
            }
            else {
                self.fonctionLieuViewModel.lieux([])
            }
            getFonction(self.typeLieuId());
            self.lieuId(null);
            self.fonctionId(null);
            self.nomLieu(null);
            self.selectTypeLieu();
        }

        self.initFonction = function () {
            self.fonctionLieuViewModel.fonctions.removeAll();
            self.fonctionLieuViewModel.fonctions.unshift({ id: null, nom: 'Tous' });
        }

        self.lieuChanged = function () {
            getFonction(self.typeLieuId(), self.lieuId())
        }

        var getLieu = function (typeLieuId) {

            $.ajax({
                url: '/lieu/get?typeId=' + typeLieuId,
                success: function (result) {                   
                    self.fonctionLieuViewModel.lieux(ko.mapping.fromJS(result)())
                    self.fonctionLieuViewModel.lieux.unshift({ id: null, nom: 'Tous' });
                    self.initFonction();
                }
            });
        }

        self.autoCompleteLieu = function (searchTerm, callback) {
            $.ajax({
                dataType: "json",
                url: "/lieu/autocomplete",
                data: {
                    typeLieuId: self.typeLieuId(),
                    filtre: searchTerm,
                    nombreAutoComplete: 10
                },
            }).done(callback);
        }

        var getFonction = function (typeLieuId, lieuId) {
            
            lieuId = lieuId || '';
            typeLieuId = typeLieuId || '';

            $.ajax({
                url: '/fonction/get?typeLieuId=' + typeLieuId + "&lieuId=" + lieuId,
                type: 'DELETE',
                success: function (result) {
                    self.fonctionLieuViewModel.fonctions(ko.mapping.fromJS(result)())
                    self.fonctionLieuViewModel.fonctions.unshift({ id: null, nom: 'Toutes' });
                }
            });
        }

        self.init = function () {

            $.extend(self, ko.mapping.fromJS(JSON.parse($(".script-server-data", jqHtmlModuleDiv).html())));

            self.fonctionLieuViewModel.lieux.unshift({ id: null, nom: 'Tous' });
            self.fonctionLieuViewModel.fonctions.unshift({ id: null, nom: 'Toutes' });

            self.typeLieu = ko.observable(self.fonctionLieuViewModel.typesLieu().find(function (l) { return l.id() == self.typeLieuId(); }));
        }

        self.selectTypeLieu = function () {
            // selection du typeLieuSelectionne
            self.typeLieu(self.fonctionLieuViewModel.typesLieu().find(function (l) { return l.id() == self.typeLieuId(); }))
        }

        self.init();
    }

    var vm = new FonctionLieuViewModel();

    ko.applyBindings(vm, jqHtmlModuleDiv);
})();

