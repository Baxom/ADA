using ADA.Data.Context;
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
            IPretreRepository pretres,
            IGenericRepository<Lieu> lieux,
            IGenericRepository<Acte> actes,
            IGenericRepository<Bibliotheque> bibliotheque,
            IGenericRepository<FonctionTypeLieu> fonctionsTypeLieu,
            IGenericRepository<FonctionLieu> fonctionsLieu,
            IGenericRepository<Fonction> fonctions,
            IGenericRepository<Commune> communes,
            IGenericRepository<TypeLieu> typeLieu,
            IGenericRepository<Revue> revues,
            IGenericRepository<ArticleRevue> articlesRevue)
        {
            _context = context;
            Pretres = pretres;
            Lieux = lieux;
            Bibliotheques = bibliotheque;
            FonctionsLieu = fonctionsLieu;
            FonctionsTypeLieu = fonctionsTypeLieu;
            Fonctions = fonctions;
            TypeLieu = typeLieu;
            Communes = communes;
            Revues = revues;
            ArticlesRevue = articlesRevue;
            Actes = actes;
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

        #endregion

        public IGenericRepository<Acte> Actes { get; private set; }
        public IGenericRepository<ArticleRevue> ArticlesRevue { get; private set; }

        public IGenericRepository<Bibliotheque> Bibliotheques { get; private set; }

        public IGenericRepository<Commune> Communes { get; private set; }
        
        public IGenericRepository<Fonction> Fonctions { get; private set; }
        public IGenericRepository<FonctionLieu> FonctionsLieu { get; private set; }
        public IGenericRepository<FonctionTypeLieu> FonctionsTypeLieu { get; private set; }

        public IGenericRepository<Lieu> Lieux { get; private set; }

        public IPretreRepository Pretres { get; private set; }

        public IGenericRepository<Revue> Revues { get; private set; }

        public IGenericRepository<TypeLieu> TypeLieu { get; private set; }

    }
}
