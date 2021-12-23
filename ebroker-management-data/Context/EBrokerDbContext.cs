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
            // Use Fluent API to configure  

            // Map entities to tables  
            modelBuilder.Entity<TraderProfile>().ToTable("Trader");
            modelBuilder.Entity<Equity>().ToTable("Equity");
            modelBuilder.Entity<TraderEquity>().ToTable("TraderEquity");

            // Configure Primary Keys  
            modelBuilder.Entity<TraderProfile>().HasKey(tp => tp.TraderId).HasName("PK_Trader");
            modelBuilder.Entity<Equity>().HasKey(e => e.EquityId).HasName("PK_Equity");
            modelBuilder.Entity<TraderEquity>().HasKey(te => te.TraderEquityId).HasName("PK_TraderEquity");

            // Configure columns  
            modelBuilder.Entity<TraderProfile>().Property(ug => ug.TraderId).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<TraderProfile>().Property(ug => ug.TraderName).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<TraderProfile>().Property(ug => ug.TraderCode).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<TraderProfile>().Property(ug => ug.Funds).HasColumnType("double");

            modelBuilder.Entity<Equity>().Property(ug => ug.EquityId).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Equity>().Property(ug => ug.EquityName).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Equity>().Property(ug => ug.EquityCode).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Equity>().Property(ug => ug.Quantity).HasColumnType("int");
            modelBuilder.Entity<Equity>().Property(ug => ug.Price).HasColumnType("double");

            modelBuilder.Entity<TraderEquity>().Property(ug => ug.TraderEquityId).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<TraderEquity>().Property(ug => ug.TraderId).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<TraderEquity>().Property(ug => ug.EquityId).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<TraderEquity>().Property(ug => ug.Quantity).HasColumnType("int");
        }
    }
}
