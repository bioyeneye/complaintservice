using Microsoft.EntityFrameworkCore;

namespace ComplaintService.DataAccess.RepositoryPattern.Interfaces
{
    public interface IRepositoryInjection
    {
        IRepositoryInjection SetContext(DbContext context);
    }
}
