using ADA.Data.Repositories.Common;
using ADA.Data.Repositories.Interfaces;
using ADA.Domain.Bibliotheques;
using ADA.Domain.Catalogues;
using ADA.Domain.Contexthistoriques;
using ADA.Domain.Fonctions;
using ADA.Domain.Fonds;
using ADA.Domain.Lieux;
using ADA.Domain.Media;
using ADA.Domain.References;
using ADA.Domain.RegistresParoissiaux;
using ADA.Domain.Revues;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ADA.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Save();
        Task SaveAsync();
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        void Rollback();
        void Commit();

        IGenericRepository<Acte> Actes { get; }
        IGenericRepository<ArticleRevue> ArticlesRevue { get; }

        IGenericRepository<Bibliotheque> Bibliotheques { get; }

        IGenericRepository<Catalogue> Catalogues { get; }
        IGenericRepository<Commune> Communes { get; }
        IGenericRepository<ContextHistorique> ContextHistoriques { get; }

        IGenericRepository<Fonction> Fonctions { get; }
        IGenericRepository<FonctionLieu> FonctionsLieu { get; }
        IGenericRepository<FonctionTypeLieu> FonctionsTypeLieu { get; }
        IFondMediumRepository FondMediums { get; }
        IGenericRepository<Fond> Fonds { get; }

        IGenericRepository<InformationFondMedium> InformationFondMedia { get; }

        IGenericRepository<Lieu> Lieux { get; }

        IGenericRepository<Medium> Media { get; }

        IPretreRepository Pretres { get; }

        IGenericRepository<Revue> Revues { get; }

        IGenericRepository<Serie> Series { get; }

        IGenericRepository<TypeLieu> TypeLieu { get; }
    }
}
