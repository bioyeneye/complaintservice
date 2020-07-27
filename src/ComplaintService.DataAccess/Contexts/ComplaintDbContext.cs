using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ComplaintService.DataAccess.Entities;
using ComplaintService.DataAccess.RepositoryPattern;

namespace ComplaintService.DataAccess.Contexts
{
    public partial class ComplaintDbContext : EntityFrameworkDataContext<ComplaintDbContext>
    {

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
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasIndex(e => e.ComplaintId);

                entity.Property(e => e.CommentBy).IsRequired();

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Message).IsRequired();

                entity.HasOne(d => d.Complaint)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ComplaintId);
            });

            modelBuilder.Entity<Complaint>(entity =>
            {
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
