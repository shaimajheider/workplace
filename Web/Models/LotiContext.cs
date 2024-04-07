using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Vue.Models
{
    public partial class LotiContext : DbContext
    {
        public LotiContext()
        {
        }

        public LotiContext(DbContextOptions<LotiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Applications> Applications { get; set; }
        public virtual DbSet<ApplicationsAttachments> ApplicationsAttachments { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<Facilities> Facilities { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=DESKTOP-G795488;database=Loti;uid=Ahmed;pwd=35087124567Ahmed;Trusted_Connection=false;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Applications>(entity =>
            {
                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DoctorName).HasMaxLength(500);

                entity.Property(e => e.FatherName).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(250);

                entity.Property(e => e.GrandFatherName).HasMaxLength(50);

                entity.Property(e => e.Levels).HasComment(@"1-Not Compleate 
2-Confirm Phone
3-Requested
4-Accepted
5-Rejected
");

                entity.Property(e => e.Nid).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.RejectOn).HasColumnType("datetime");

                entity.Property(e => e.SirName).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasDefaultValueSql("((0))")
                    .HasComment(@"1-active
2-request
3-stopped
9-delete
");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Applications_Cities");

                entity.HasOne(d => d.Facility)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.FacilityId)
                    .HasConstraintName("FK_Applications_Facilities");
            });

            modelBuilder.Entity<ApplicationsAttachments>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(550);

                entity.Property(e => e.Path).HasMaxLength(550);

                entity.Property(e => e.Status)
                    .HasDefaultValueSql("((0))")
                    .HasComment(@"1-active
2-request
3-stopped
9-delete
");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationsAttachments)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("FK_ApplicationsAttachments_Applications");
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.Property(e => e.ArabicName).HasMaxLength(250);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EnglishName).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Status)
                    .HasDefaultValueSql("((0))")
                    .HasComment(@"1-active
2-request
3-stopped
9-delete
");
            });

            modelBuilder.Entity<ContactUs>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(50);
            });

            modelBuilder.Entity<Facilities>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Status)
                    .HasDefaultValueSql("((0))")
                    .HasComment(@"1-active
2-request
3-stopped
9-delete
");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Facilities)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Facilities_Cities");
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.Property(e => e.Controller).HasMaxLength(250);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Operations)
                    .HasMaxLength(50)
                    .HasComment(@"1-Add
2-Edit
3-Delete
4-ChangeStatus
5-Other");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(555);

                entity.Property(e => e.LastLoginOn).HasColumnType("datetime");

                entity.Property(e => e.LoginName).HasMaxLength(50);

                entity.Property(e => e.LoginTryAttemptDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(25);

                entity.Property(e => e.Status)
                    .HasDefaultValueSql("((0))")
                    .HasComment(@"1-active
2-request
3-stopped
9-delete
");

                entity.Property(e => e.UserType).HasDefaultValueSql("((2))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
