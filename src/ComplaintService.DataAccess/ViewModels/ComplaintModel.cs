using System;
using System.ComponentModel.DataAnnotations;
using ComplaintService.DataAccess.Entities.enums;
using Newtonsoft.Json;

namespace ComplaintService.DataAccess.ViewModels
{
    public class ComplaintModel
    {
        [JsonIgnore] public string Id { get; set; }

        [Required] public ComplaintServiceCategory Category { get; set; }

        [Required] public string Summary { get; set; }

        [Required] public string Description { get; set; }

        [Required] public ComplaintType Type { get; set; }

        [JsonIgnore] public string ComplainBy { get; set; }

        [JsonIgnore] public DateTime DateCreated { get; set; }

        [JsonIgnore] public ComplaintStatus Status { get; set; }
    }

    public class ComplaintItem : ComplaintModel
    {
    }

    public class ComplaintFilter : ComplaintItem
    {
        public DateTime? DateCreatedFrom { get; set; }
        public DateTime? DateCreatedTo { get; set; }

        public static ComplaintFilter Deserialize(string whereCondition)
        {
            ComplaintFilter filter = null;
            if (whereCondition != null) filter = JsonConvert.DeserializeObject<ComplaintFilter>(whereCondition);
            return filter;
        }
    }
}