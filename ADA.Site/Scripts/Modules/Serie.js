(function () {

    var jqHtmlModuleDiv = $("#recherche-catalogue")[0];

    function SerieViewModel() {
        var self = this;

        self.init = function () {

            $.extend(self, ko.mapping.fromJS(JSON.parse($(".script-server-data", jqHtmlModuleDiv).html())));

            var localSousSeriesArray = self.series().map(function (s) {

                s.sousSeries().unshift({ id: null, nom: 'Toutes' });
                return s.sousSeries();
            })

            var localSousSeries = [].concat.apply([], localSousSeriesArray);
            

            var sousSerie = localSousSeries.find(function (ss) {
                return ss.id && ss.id() === self.sousSerieId()
            });

            if (sousSerie) self.serieId = sousSerie.serieId;

            var serie = self.series().find(function (x) {
                return x.id() == self.serieId();
            });

            var sousSeries = serie && serie.sousSeries() || [];

            self.sousSeries = ko.observableArray(sousSeries);
           // self.sousSeries.unshift({ id: null, nom: 'Toutes' });
            self.series.unshift({ id: null, nom: 'Toutes' });
        }

        self.serieChanged = function () {
            var serie = self.series().find(function (s) {
                return s.id != null && s.id() === self.serieId();
            });

            var sousSeries = serie && serie.sousSeries() || [];
           // sousSeries.unshift({ id: null, nom: 'Toutes' });

            self.sousSeries(sousSeries);
            
            self.sousSerieId(null);
        }

        self.init();
    }

    var vm = new SerieViewModel();

    ko.applyBindings(vm, jqHtmlModuleDiv);
})();

