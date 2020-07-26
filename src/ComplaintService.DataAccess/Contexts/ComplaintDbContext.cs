using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ComplaintService.DataAccess.Entities;

namespace ComplaintService.DataAccess.Contexts
{
    public partial class ComplaintDbContext : DbContext
    {
        public ComplaintDbContext()
        {
        }

        public ComplaintDbContext(DbContextOptions<ComplaintDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Complaint> Complaints { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("Data Source= /Users/oyeneyebolaji/RiderProjects/banklyservices/src/ComplaintService/ComplaintService/complaint.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Commentby).HasColumnName("commentby");

                entity.Property(e => e.Complaintid).HasColumnName("complaintid");

                entity.Property(e => e.Message).HasColumnName("message");

                entity.HasOne(d => d.Complaint)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.Complaintid);
            });

            modelBuilder.Entity<Complaint>(entity =>
            {
                entity.Property(e => e.DateCreated)
                    .IsRequired()
                    .HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
