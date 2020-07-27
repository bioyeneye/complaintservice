using System;
using System.Data;

namespace ComplaintService.DataAccess.RepositoryPattern.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        void Dispose(bool disposing);
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        void Commit();
        void Rollback();

        // IEnumerable<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
        //     where TEntity : class , new();
        //
        // IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        //     where TElement : class;
    }
}