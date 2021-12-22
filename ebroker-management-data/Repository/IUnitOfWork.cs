using EBroker.Management.Data.Context;
using System;

namespace EBroker.Management.Data.Repository
{
    public interface IUnitOfWork<out TContext> : IDisposable where TContext : IDbContextBase
    {
        protected internal IDbContext Context { get; }

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        void BeginTransaction();
        bool Commit();
        void Rollback();
    }
}
