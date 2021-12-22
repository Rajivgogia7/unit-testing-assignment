using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace EBroker.Management.Data.Context
{
    public interface IDbContextBase
    {
        int SaveChanges();

        void Dispose();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DatabaseFacade Database { get; }
    }
}
