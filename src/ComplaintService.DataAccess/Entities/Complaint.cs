using System;
using System.Collections.Generic;

namespace ComplaintService.DataAccess.Entities
{
    public partial class Complaint
    {
        public Complaint()
        {
            Comments = new HashSet<Comment>();
        }

        public string Id { get; set; }
        public string ComplainBy { get; set; }
        public int Category { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public DateTime DateCreated { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
