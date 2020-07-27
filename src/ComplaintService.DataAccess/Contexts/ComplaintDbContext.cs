using System;
using ComplaintService.DataAccess.Entities;
using ComplaintService.DataAccess.RepositoryPattern;
using Microsoft.EntityFrameworkCore;

namespace ComplaintService.DataAccess.Contexts
{
    public class ComplaintDbContext : EntityFrameworkDataContext<ComplaintDbContext>
    {
        public ComplaintDbContext(DbContextOptions<ComplaintDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Complaint> Complaints { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}