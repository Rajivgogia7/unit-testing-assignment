using EBroker.Management.Domain.Trading;
using EBroker.Management.Domain.Equity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Data.Context
{
    public class EBrokerDbContext : DbContext, IEBrokerDbContext, IDbContext
    {
        public virtual DbSet<TraderProfile> Traders { get; set; }
        public virtual DbSet<Equity> Equities { get; set; }
        public virtual DbSet<TraderEquity> TraderEquity { get; set; }

        [ExcludeFromCodeCoverage]
        public EBrokerDbContext()
        {
        }

        [ExcludeFromCodeCoverage]
        protected EBrokerDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public EBrokerDbContext(DbContextOptions<EBrokerDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
