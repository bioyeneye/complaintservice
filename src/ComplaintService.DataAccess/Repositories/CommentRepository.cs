using ComplaintService.DataAccess.Entities;
using ComplaintService.DataAccess.RepositoryPattern;
using ComplaintService.DataAccess.RepositoryPattern.Interfaces;

namespace ComplaintService.DataAccess.Repositories
{
    public interface ICommentRepository : IRepositoryAsync<Comment>
    {
    }


    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(IDataContextAsync context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
    }
}