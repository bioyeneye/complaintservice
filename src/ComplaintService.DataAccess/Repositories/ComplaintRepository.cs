using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ComplaintService.DataAccess.Entities;
using ComplaintService.DataAccess.RepositoryPattern;
using ComplaintService.DataAccess.RepositoryPattern.Interfaces;
using ComplaintService.DataAccess.ViewModels;
using CoreLibrary.DataContext;

namespace ComplaintService.DataAccess.Repositories
{
    public interface IComplaintRepository : IRepositoryAsync<Complaint>
    {
        IEnumerable<Complaint> GetComplaintPaged(int page, int count, out int totalCount,
            ComplaintFilter filter = null, OrderExpression orderExpression = null);
        IEnumerable<Complaint> GetComplaintPaged(int page, int count, ComplaintFilter filter = null,
            OrderExpression orderExpression = null);
        IEnumerable<Complaint> GetComplaintFilteredQueryable(ComplaintFilter filter = null);
    }


    public class ComplaintRepository : Repository<Complaint>, IComplaintRepository
    {
        public ComplaintRepository(IDataContextAsync context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public IEnumerable<Complaint> GetComplaintPaged(int page, int count, out int totalCount,
            ComplaintFilter filter = null, OrderExpression orderExpression = null)
        {
            var expression = new ComplaintQueryObject(filter).Expression;
            totalCount = Count(expression);
            return ComplaintPaged(page, count, expression, orderExpression);
        }
        public IEnumerable<Complaint> GetComplaintPaged(int page, int count, ComplaintFilter filter = null, OrderExpression orderExpression = null)
        {
            var expression = new ComplaintQueryObject(filter).Expression;
            return ComplaintPaged(page, count, expression, orderExpression);
        }

        private IEnumerable<Complaint> ComplaintPaged(int page, int count, Expression<Func<Complaint, bool>> expression,
            OrderExpression orderExpression)
        {
            var order = ProcessOrderFunc(orderExpression);
            return Fetch(expression, order, page, count);
        }

        public IEnumerable<Complaint> GetComplaintFilteredQueryable(ComplaintFilter filter = null)
        {
            var expression = new ComplaintQueryObject(filter).Expression;
            return Fetch(expression);
        }

        

        public static Func<IQueryable<Complaint>, IOrderedQueryable<Complaint>> ProcessOrderFunc(OrderExpression orderDeserilizer = null)
        {
            Func<IQueryable<Complaint>, IOrderedQueryable<Complaint>> orderFuction = (queryable) =>
            {
                var orderQueryable = queryable.OrderByDescending(x => x.DateCreated);
                if (orderDeserilizer != null)
                {
                    switch (orderDeserilizer.Column)
                    {

                    }
                }
                return orderQueryable;
            };
            return orderFuction;
        }

        public class ComplaintQueryObject : QueryObject<Complaint>
        {
            public ComplaintQueryObject(ComplaintFilter filter)
            {
                if (filter != null)
                {

                }
            }
        }
    }
}