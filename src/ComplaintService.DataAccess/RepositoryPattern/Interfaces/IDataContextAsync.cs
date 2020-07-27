using System.Threading;
using System.Threading.Tasks;

namespace ComplaintService.DataAccess.RepositoryPattern.Interfaces
{
    public interface IDataContextAsync : IDataContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();
    }
}