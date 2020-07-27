using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ComplaintService.BusinessDomain.ApplicationModels;
using ComplaintService.DataAccess.Entities;
using ComplaintService.DataAccess.Entities.enums;
using ComplaintService.DataAccess.Repositories;
using ComplaintService.DataAccess.RepositoryPattern;
using ComplaintService.DataAccess.RepositoryPattern.Interfaces;
using ComplaintService.DataAccess.ViewModels;

namespace ComplaintService.BusinessDomain.Services
{
    public interface IComplaintService
    {
        void Create(ComplaintModel model, string username);
        void Edit(ComplaintModel model);
        void Delete(string id);
        ComplaintItem GetById(string id);
        ComplaintItem GetDetailsById(string id);
        IEnumerable<ComplaintModel> Query(int page, int count, ComplaintFilter filter, string orderByExpression = null);
        CountModel<ComplaintItem> GetCount(int page, int count, ComplaintFilter filter, string orderByExpression = null);
        void UpdateComplaint(ComplaintModel user, string userid);
        void ToggleActive(string id, string username);
        bool ComplaintExists(ComplaintModel model);
    }
    
    public class ComplaintService : IComplaintService
    {
        private readonly IComplaintRepository _repository;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private IMapper _mapper;

        public ComplaintService(IComplaintRepository repository, IUnitOfWorkAsync unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Create(ComplaintModel model, string username)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var complaint = new Complaint
                {
                    Id = Guid.NewGuid().ToString(),
                    Category = (int)model.Category,
                    Description = model.Description,
                    Summary = model.Summary,
                    DateCreated = DateTime.UtcNow,
                    Status = (int)ComplaintStatus.Created,
                    Type = (int)model.Type,
                };
                _repository.Insert(complaint);
                _unitOfWork.Commit();
            }
            catch (System.Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }

        public void Edit(ComplaintModel model)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var complaint = GetComplaintEntity(model.Id);
                if (complaint == null) throw new Exception("Complaint record not found");

                _mapper.Map(model, complaint);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw; //new Exception(ex.Message);
            }
        }

        public Complaint GetComplaintEntity(string id)
        {
            return _repository.Find(c => c.Id == id);
        }

        public async void Delete(string id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var entity = GetComplaintEntity(id);
                if(entity == null) throw new Exception($"Complaint not found");
                await _repository.DeleteAsync(entity);
                await _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public ComplaintItem GetById(string id)
        {
            var entity = GetComplaintEntity(id);
            var returnEntity =
                _mapper.Map<Complaint, ComplaintItem>(entity);
            return returnEntity;
        }

        public ComplaintItem GetDetailsById(string id)
        {
            var entity = GetComplaintEntity(id);
            var item = _mapper.Map<Complaint, ComplaintItem>(entity);
            return item;
        }
        
        private IEnumerable<ComplaintItem> ProcessQuery(
            IEnumerable<Complaint> entities)
        {
            return entities.Select(c =>
            {
                var item =
                    _mapper.Map<Complaint, ComplaintItem>(c);
                return item;
            });
        }

        public IEnumerable<ComplaintModel> Query(int page, int count, ComplaintFilter filter, string orderByExpression = null)
        {
            var orderBy = OrderExpression.Deserializer(orderByExpression);
            var entities =
                _repository.GetComplaintPaged(page,
                    count, filter, orderBy);
            return ProcessQuery(entities);
        }

        public CountModel<ComplaintItem> GetCount(int page, int count, ComplaintFilter filter, string orderByExpression = null)
        {
            int totalCount;
            var orderBy = OrderExpression.Deserializer(orderByExpression);
            var entities =
                _repository.GetComplaintPaged(page,
                    count, out totalCount, filter, orderBy);

            return new CountModel<ComplaintItem>
            {
                Total = totalCount,
                Items = ProcessQuery(entities)
            };
        }

        public void UpdateComplaint(ComplaintModel model, string userid)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var complaint = GetComplaintEntity(model.Id);
                if (complaint == null) throw new Exception("Complaint record not found");

                _mapper.Map(model, complaint);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw; //new Exception(ex.Message);
            }
        }

        public void ToggleActive(string id, string username)
        {
            throw new System.NotImplementedException();
        }

        public bool ComplaintExists(ComplaintModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}