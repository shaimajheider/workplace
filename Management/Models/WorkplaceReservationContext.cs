using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Vue.Models
{
    public partial class WorkplaceReservationContext : DbContext
    {
        public WorkplaceReservationContext()
        {
        }

        public WorkplaceReservationContext(DbContextOptions<WorkplaceReservationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClassRoomReservations> ClassRoomReservations { get; set; }
        public virtual DbSet<CompainesAttachmenets> CompainesAttachmenets { get; set; }
        public virtual DbSet<CompainesRoomAttachments> CompainesRoomAttachments { get; set; }
        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<CompaniesRooms> CompaniesRooms { get; set; }
        public virtual DbSet<CompaniesSchedule> CompaniesSchedule { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<Offers> Offers { get; set; }
        public virtual DbSet<PaymentMethods> PaymentMethods { get; set; }
        public virtual DbSet<Subscriptions> Subscriptions { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<UserSuspends> UserSuspends { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Wallet> Wallet { get; set; }
        public virtual DbSet<WalletPurchases> WalletPurchases { get; set; }
        public virtual DbSet<WalletTransactions> WalletTransactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=DESKTOP-G795488;database=WorkplaceReservation;uid=Ahmed;pwd=35087124567Ahmed;Trusted_Connection=false;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassRoomReservations>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.Status).HasComment(@"1-active
2-not active
3-stopped
4-admin
9-delete
");

                entity.HasOne(d => d.CompaniesRoom)
                    .WithMany(p => p.ClassRoomReservations)
                    .HasForeignKey(d => d.CompaniesRoomId)
                    .HasConstraintName("FK_ClassRoomReservations_CompaniesRooms");

                entity.HasOne(d => d.Subscriptions)
                    .WithMany(p => p.ClassRoomReservations)
                    .HasForeignKey(d => d.SubscriptionsId)
                    .HasConstraintName("FK_ClassRoomReservations_Subscriptions");
            });

            modelBuilder.Entity<CompainesAttachmenets>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Path).HasMaxLength(250);

                entity.Property(e => e.Status).HasComment(@"1-active
2-not active
3-stopped
4-admin
9-delete
");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompainesAttachmenets)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_CompainesAttachmenets_Companies");
            });

            modelBuilder.Entity<CompainesRoomAttachments>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Path).HasMaxLength(250);

                entity.Property(e => e.Status).HasComment(@"1-active
2-not active
3-stopped
4-admin
9-delete
");

                entity.HasOne(d => d.CompanyRoom)
                    .WithMany(p => p.CompainesRoomAttachments)
                    .HasForeignKey(d => d.CompanyRoomId)
                    .HasConstraintName("FK_CompainesRoomAttachments_CompaniesRooms");
            });

            modelBuilder.Entity<Companies>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.LocationDescriptions).HasMaxLength(500);

                entity.Property(e => e.LocationLink).HasMaxLength(500);

                entity.Property(e => e.OwnerName).HasMaxLength(400);

                entity.Property(e => e.OwnerPhone).HasMaxLength(50);

                entity.Property(e => e.Status).HasComment(@"1-active
2-not active
3-stopped
4-admin
9-delete
");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Companies)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Companies_Users");
            });

            modelBuilder.Entity<CompaniesRooms>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Status).HasComment(@"1-active
2-not active
3-stopped
4-admin
9-delete
");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompaniesRooms)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_CompaniesRooms_Companies");
            });

            modelBuilder.Entity<CompaniesSchedule>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.From).HasColumnType("datetime");

                entity.Property(e => e.To).HasColumnType("datetime");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompaniesSchedule)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_CompaniesSchedule_Companies");
            });

            modelBuilder.Entity<ContactUs>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(50);
            });

            modelBuilder.Entity<Offers>(entity =>
            {
                entity.Property(e => e.AcceptedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.LenthType).HasComment(@"1-Days
2-Month
3-Years");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.RejectedOn).HasColumnType("datetime");

                entity.Property(e => e.Status).HasComment(@"1-active
2-not active
3-stopped
4-admin
9-delete
");

                entity.Property(e => e.Target).HasComment(@"1-Studnet
2-Copmanies
3-both");

                entity.HasOne(d => d.CompaniesRoom)
                    .WithMany(p => p.Offers)
                    .HasForeignKey(d => d.CompaniesRoomId)
                    .HasConstraintName("FK_Offers_CompaniesRooms");
            });

            modelBuilder.Entity<PaymentMethods>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Status).HasComment("1-active 2-not active 9-delete ");
            });

            modelBuilder.Entity<Subscriptions>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.End).HasColumnType("datetime");

                entity.Property(e => e.LastPaymentDate).HasColumnType("datetime");

                entity.Property(e => e.Start).HasColumnType("datetime");

                entity.Property(e => e.Status).HasComment(@"1-active
2-not active
3-stopped
4-admin
9-delete
");

                entity.HasOne(d => d.Offer)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.OfferId)
                    .HasConstraintName("FK_SubscriptionsRequest_Offers");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_SubscriptionsRequest_Users");
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

            modelBuilder.Entity<UserSuspends>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Status).HasComment(@"1-active
2-not active
3-stopped
4-admin
9-delete
");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSuspends)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserSuspends_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(70);

                entity.Property(e => e.Image).HasMaxLength(500);

                entity.Property(e => e.LastLoginOn).HasColumnType("datetime");

                entity.Property(e => e.Level).HasComment(@"1-Request Join
2-Accepted
3-Rejected");

                entity.Property(e => e.LoginName).HasMaxLength(50);

                entity.Property(e => e.LoginTryAttemptDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(400);

                entity.Property(e => e.Password).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(12);

                entity.Property(e => e.Status).HasComment(@"1-active
2-not active
3-stopped
4-admin
9-delete
");

                entity.Property(e => e.UserType)
                    .HasDefaultValueSql("((2))")
                    .HasComment(@"1-admin
2- Company
3-finder




");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Wallet)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Wallet_Users");
            });

            modelBuilder.Entity<WalletPurchases>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Subscriptions)
                    .WithMany(p => p.WalletPurchases)
                    .HasForeignKey(d => d.SubscriptionsId)
                    .HasConstraintName("FK_WalletPurchases_Subscriptions");

                entity.HasOne(d => d.Wallet)
                    .WithMany(p => p.WalletPurchases)
                    .HasForeignKey(d => d.WalletId)
                    .HasConstraintName("FK_WalletPurchases_Wallet");
            });

            modelBuilder.Entity<WalletTransactions>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ProcessType).HasComment(@"1-recharge
");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.WalletTransactions)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .HasConstraintName("FK_WalletTransactions_PaymentMethods");

                entity.HasOne(d => d.Wallet)
                    .WithMany(p => p.WalletTransactions)
                    .HasForeignKey(d => d.WalletId)
                    .HasConstraintName("FK_WalletTransactions_Wallet");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
