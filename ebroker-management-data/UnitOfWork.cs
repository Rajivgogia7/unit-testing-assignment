using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EBroker.Management.Data
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : IDbContextBase
    {
        private readonly IDbContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private bool _disposed;
        private IDbContextTransaction _transaction;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetService(typeof(TContext)) as IDbContext;
            _repositories = new Dictionary<Type, object>();
            _disposed = false;
            _serviceProvider = serviceProvider;
        }

        IDbContext IUnitOfWork<TContext>.Context => this._context;

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            // Checks if the Dictionary Key contains the Model class
            if (_repositories.Keys.Contains(typeof(TEntity)))
            {
                // Return the repository for that Model class
                return _repositories[typeof(TEntity)] as IRepository<TEntity>;
            }

            // If the repository for that Model class doesn't exist, create it
            Repository<TEntity> repository = new Repository<TEntity>(_context);

            // Add it to the dictionary
            _repositories.Add(typeof(TEntity), repository);

            return repository;
        }

        public bool Commit()
        {
            if (_disposed)
                return false;

            int entriesCommitted = _context.SaveChanges();

            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction = null;
            }
            return entriesCommitted > 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _context.Dispose();
                _transaction?.Dispose();
            }

            _disposed = true;
        }

        public void BeginTransaction()
        {
            if (_disposed)
                return;

            var context = _context;

            if (_transaction == null)
                _transaction = context.Database.BeginTransaction();

        }
        public void Rollback()
        {
            if (_disposed)
                return;

            _transaction?.Rollback();
            _transaction?.Dispose();
        }
    }
}
