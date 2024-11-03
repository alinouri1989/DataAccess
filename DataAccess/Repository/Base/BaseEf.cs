using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Repository.Base
{
    public class BaseEf : IBaseEf
    {
        private IDbContextTransaction _transaction;
        private readonly DbContext _dbContext;

        public BaseEf(DbContext dbContext)
        {
            _dbContext = dbContext;
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