using EBroker.Management.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System;
using Xunit;

namespace EBroker.Management.Data.Tests
{
    public class UnitOfWorkTests
    {
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly UnitOfWork<IDbContext> _unitOfWork;
        private readonly Mock<IDbContext> _context;

        public UnitOfWorkTests()
        {
            _context = new Mock<IDbContext>();
            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockServiceProvider.Setup(x => x.GetService(typeof(IDbContext))).Returns(_context.Object);
            
            _unitOfWork = new UnitOfWork<IDbContext>(_mockServiceProvider.Object);
        }

        [Fact]
        public void ShouldCreateRepository()
        {
            _context.Setup(x => x.Set<object>()).Returns(new Mock<DbSet<object>>().Object);

            var repo = _unitOfWork.GetRepository<object>();

            Assert.NotNull(repo);

            var newRepo = _unitOfWork.GetRepository<object>();

            Assert.NotNull(repo);
            Assert.True(object.ReferenceEquals(repo, newRepo));
        }

        [Fact]
        public void ShouldBeginTransactionOnlyWhenNotDisposed()
        {
            var db = new Mock<DatabaseFacade>(new Mock<DbContext>().Object);
            db.Setup(x => x.EnsureCreated()).Returns(true);
            db.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);

            _context.SetupGet(x => x.Database).Returns(db.Object);

            _unitOfWork.BeginTransaction();

            db.Verify(x => x.BeginTransaction(), Times.Once);

            _unitOfWork.BeginTransaction();

            db.Verify(x => x.BeginTransaction(), Times.Once);

            _unitOfWork.Dispose();
            _unitOfWork.BeginTransaction();

            db.Verify(x => x.BeginTransaction(), Times.Once);
        }

        [Fact]
        public void ShouldRollbackTransactionOnlyWhenNotDisposed()
        {
            var transaction = new Mock<IDbContextTransaction>();

            var db = new Mock<DatabaseFacade>(new Mock<DbContext>().Object);
            db.Setup(x => x.EnsureCreated()).Returns(true);
            db.Setup(x => x.BeginTransaction()).Returns(transaction.Object);

            _context.SetupGet(x => x.Database).Returns(db.Object);

            _unitOfWork.BeginTransaction();
            _unitOfWork.Rollback();

            _unitOfWork.BeginTransaction();
            _unitOfWork.Dispose();
            _unitOfWork.Rollback();

            transaction.Verify(x => x.Rollback(), Times.Once);
            transaction.Verify(x => x.Dispose(), Times.Exactly(2));
        }

        [Fact]
        public void ShouldCommitTransaction()
        {
            var transaction = new Mock<IDbContextTransaction>();

            var db = new Mock<DatabaseFacade>(new Mock<DbContext>().Object);
            db.Setup(x => x.EnsureCreated()).Returns(true);
            db.Setup(x => x.BeginTransaction()).Returns(transaction.Object);

            _context.SetupGet(x => x.Database).Returns(db.Object);
            _context.Setup(x => x.SaveChanges()).Returns(4);

            _unitOfWork.BeginTransaction();
            var result = _unitOfWork.Commit();

            Assert.True(result);

            transaction.Verify(x => x.Commit(), Times.Once);

            _unitOfWork.Dispose();
            result = _unitOfWork.Commit();

            Assert.False(result);
        }
    }
}
