using AutoMapper;
using ComplaintService.DataAccess.Entities;
using ComplaintService.DataAccess.ViewModels;

namespace ComplaintService
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            ComplaintProfiling();
        }

        private void ComplaintProfiling()
        {
            CreateMap<Complaint, ComplaintModel>();
            CreateMap<ComplaintModel, Complaint>();
            CreateMap<Complaint, ComplaintItem>();
            CreateMap<ComplaintItem, Complaint>();
        }
    }
}