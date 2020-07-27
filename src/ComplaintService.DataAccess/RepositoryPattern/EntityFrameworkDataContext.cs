using System;
using System.Threading;
using System.Threading.Tasks;
using ComplaintService.DataAccess.RepositoryPattern.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComplaintService.DataAccess.RepositoryPattern
{
    public class EntityFrameworkDataContext<TContext> : DbContext, IDataContextAsync where TContext : DbContext
    {
        #region Private Fields

        private readonly Guid _instanceId;

        #endregion Private Fields

        public EntityFrameworkDataContext(DbContextOptions<TContext> options)
            : base(options)
        {
            _instanceId = Guid.NewGuid();
            //Configuration.LazyLoadingEnabled = false;
            //Configuration.ProxyCreationEnabled = false;
            //Configuration.AutoDetectChangesEnabled = false;
        }


        public async Task<int> SaveChangesAsync()
        {
            return await SaveChangesAsync(CancellationToken.None);
        }
    }
}