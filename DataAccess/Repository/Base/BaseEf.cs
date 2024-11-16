using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using San.CoreCommon.ServiceActivator;
using System.Threading.Tasks;

namespace Repository.Base
{
    public class BaseEf<TContext> : IBaseEf
        where TContext : DbContext
    {
        private IDbContextTransaction _transaction;
        private readonly TContext _dbContext;

        public BaseEf()
        {
            _dbContext = ServiceActivator.ResolveService<TContext>();
        }
        public async Task BeginTransaction() => _transaction = await _dbContext.Database.BeginTransactionAsync();
        public async Task CommitTransaction() => await _transaction.CommitAsync();
        public async Task<int> SaveChangeAsync() => await _dbContext.SaveChangesAsync();
    }

    public interface IBaseEf
    {
        Task BeginTransaction();
        Task CommitTransaction();
        Task<int> SaveChangeAsync();
    }
}