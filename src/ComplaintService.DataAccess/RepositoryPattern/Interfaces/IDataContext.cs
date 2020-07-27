using System;

namespace ComplaintService.DataAccess.RepositoryPattern.Interfaces
{
    public interface IDataContext : IDisposable
    {
        int SaveChanges();
    }
}