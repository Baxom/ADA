using ADA.Data.Repositories.Common;
using ADA.Data.Repositories.Interfaces;
using ADA.Domain.Bibliotheques;
using ADA.Domain.Fonctions;
using ADA.Domain.Lieux;
using ADA.Domain.Pretres;
using ADA.Domain.References;
using ADA.Domain.RegistresParoissiaux;
using ADA.Domain.Revues;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
        
        IGenericRepository<Commune> Communes { get; }
                
        IGenericRepository<Fonction> Fonctions { get; }
        IGenericRepository<FonctionLieu> FonctionsLieu { get; }
        IGenericRepository<FonctionTypeLieu> FonctionsTypeLieu { get; }

        IGenericRepository<Lieu> Lieux { get; }

        IPretreRepository Pretres { get; }

        IGenericRepository<Revue> Revues { get; }

        IGenericRepository<TypeLieu> TypeLieu { get; }
    }
}
