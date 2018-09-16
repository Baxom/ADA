using ADA.Data.Context;
using ADA.Data.Repositories.Common;
using ADA.Data.Repositories.Interfaces;
using ADA.Domain.Bibliotheques;
using ADA.Domain.Catalogues;
using ADA.Domain.Contexthistoriques;
using ADA.Domain.Fonctions;
using ADA.Domain.Fonds;
using ADA.Domain.Lieux;
using ADA.Domain.Media;
using ADA.Domain.Pretres;
using ADA.Domain.References;
using ADA.Domain.RegistresParoissiaux;
using ADA.Domain.Revues;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContextTransaction _transactionInstance = null;
        private int _transactionCount = 0;

        private readonly ADAContext _context;

        public UnitOfWork(ADAContext context,
            IGenericRepository<Acte> actes,
            IGenericRepository<ArticleRevue> articlesRevue,
            IGenericRepository<Bibliotheque> bibliotheque,
            IGenericRepository<Catalogue> catalogues,
            IGenericRepository<Commune> communes,
            IGenericRepository<ContextHistorique> contextHistoriques,
            IGenericRepository<Fonction> fonctions,
            IGenericRepository<FonctionLieu> fonctionsLieu,
            IGenericRepository<FonctionTypeLieu> fonctionsTypeLieu,
            IFondMediumRepository fondMediums,
            IGenericRepository<Fond> fonds,
            IGenericRepository<InformationFondMedium> informationFondMedia,
            IGenericRepository<Lieu> lieux,
            IGenericRepository<Medium> media,
            IPretreRepository pretres,
            IGenericRepository<Revue> revues,
            IGenericRepository<Serie> series,
            IGenericRepository<TypeLieu> typeLieu
            )
        {
            _context = context;
            Actes = actes;
            ArticlesRevue = articlesRevue;
            Bibliotheques = bibliotheque;
            Catalogues = catalogues;
            Communes = communes;
            ContextHistoriques = contextHistoriques;
            Fonctions = fonctions;
            FonctionsLieu = fonctionsLieu;
            FonctionsTypeLieu = fonctionsTypeLieu;
            FondMediums = fondMediums;
            Fonds = fonds;
            InformationFondMedia = informationFondMedia;
            Lieux = lieux;
            Media = media;
            Pretres = pretres;
            Revues = revues;
            Series = series;
            TypeLieu = typeLieu;
        }
        
        public virtual void Save()
        {
            _context.SaveChanges();
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (_transactionCount == 0)
            {
                _transactionInstance = _context.Database.BeginTransaction(isolationLevel);
            }

            ++_transactionCount;
        }

        public void Rollback()
        {
            if (_transactionInstance != null)
            {
                _transactionInstance.Rollback();
                DisposeTransaction();
                _transactionCount = 0;
            }
        }

        public void Commit()
        {
            --_transactionCount;

            if (_transactionCount == 0)
            {
                _transactionInstance.Commit();
                DisposeTransaction();
            }
        }

        private void DisposeTransaction()
        {
            _transactionInstance.Dispose();
            _transactionInstance = null;
        }

        #region IDisposable

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public string FTSContains(string search)
        {
            if (String.IsNullOrWhiteSpace(search)) return String.Empty;

            return string.Format("({0}{1})", Intercepteurs.FtsInterceptor.FullTextPrefix, search);
        }

        #endregion

        public IGenericRepository<Acte> Actes { get; private set; }
        public IGenericRepository<ArticleRevue> ArticlesRevue { get; private set; }

        public IGenericRepository<Bibliotheque> Bibliotheques { get; private set; }

        public IGenericRepository<Catalogue> Catalogues { get; private set; }
        public IGenericRepository<Commune> Communes { get; private set; }
        public IGenericRepository<ContextHistorique> ContextHistoriques { get; private set; }

        public IGenericRepository<Fonction> Fonctions { get; private set; }
        public IGenericRepository<FonctionLieu> FonctionsLieu { get; private set; }
        public IGenericRepository<FonctionTypeLieu> FonctionsTypeLieu { get; private set; }
        public IFondMediumRepository FondMediums { get; private set; }
        public IGenericRepository<Fond> Fonds { get; private set; }

        public IGenericRepository<InformationFondMedium> InformationFondMedia { get; private set; }

        public IGenericRepository<Lieu> Lieux { get; private set; }

        public IGenericRepository<Medium> Media { get; private set; }

        public IPretreRepository Pretres { get; private set; }

        public IGenericRepository<Revue> Revues { get; private set; }

        public IGenericRepository<Serie> Series { get; private set; }

        public IGenericRepository<TypeLieu> TypeLieu { get; private set; }

    }
}

