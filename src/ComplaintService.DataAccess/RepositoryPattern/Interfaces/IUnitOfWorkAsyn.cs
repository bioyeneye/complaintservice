using System.Threading;
using System.Threading.Tasks;

namespace ComplaintService.DataAccess.RepositoryPattern.Interfaces
{
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        new Task<int> Commit();
        Task<int> CommitAsync();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class;
    }
}