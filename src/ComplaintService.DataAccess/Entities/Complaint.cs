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
        public long Category { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public long Type { get; set; }
        public byte[] DateCreated { get; set; }
        public long Status { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
