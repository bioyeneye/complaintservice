using System;

namespace ComplaintService.DataAccess.Entities
{
    public class Comment
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string CommentBy { get; set; }
        public string ComplaintId { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Complaint Complaint { get; set; }
    }
}