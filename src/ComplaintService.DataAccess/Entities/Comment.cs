using System;
using System.Collections.Generic;

namespace ComplaintService.DataAccess.Entities
{
    public partial class Comment
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string Commentby { get; set; }
        public string Complaintid { get; set; }

        public virtual Complaint Complaint { get; set; }
    }
}
