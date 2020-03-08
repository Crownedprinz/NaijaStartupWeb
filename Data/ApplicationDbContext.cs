using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using static NaijaStartupWeb.Models.NsuDtos;

namespace NaijaStartupWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }   

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Package> Package { get; set; }
        public virtual DbSet<Company_Registration> Company_Registration { get; set; }
        public virtual DbSet<Company_Officers> Company_Officers { get; set; }
        public virtual DbSet<AddOnService> AddOnService { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Incentives> Incentives { get; set; }
        public virtual DbSet<Comp_Incentives> Comp_Incentives { get; set; }
        public virtual DbSet<ChatHeader> ChatHeader { get; set; }
        public virtual DbSet<ChatThread> ChatThread { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Package>()
                   .Property(x => x.IsDeleted)
                   .HasColumnType("bit")
                   .HasColumnAnnotation("Default", 0);
            modelBuilder.Entity<Package>()
                   .Property(x => x.CreationTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Package>()
                   .Property(x => x.DeletionTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Package>()
                   .Property(x => x.ModificationUserId)
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .HasColumnAnnotation("Default", "''");
            modelBuilder.Entity<Package>()
                   .Property(x => x.CreatorUserId)
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .HasColumnAnnotation("Default", "''");
            modelBuilder.Entity<Package>()
                   .Property(x => x.ModificationUserId)
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .HasColumnAnnotation("Default", "''");
            modelBuilder.Entity<Package>()
                   .Property(x => x.DeletionUserId)
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .HasColumnAnnotation("Default", "''");
            modelBuilder.Entity<Contact>()
                   .Property(x => x.IsDeleted)
                   .HasColumnType("bit")
                   .HasColumnAnnotation("Default", 0);
            modelBuilder.Entity<Contact>()
                .Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasColumnAnnotation("Default","(getutcdate())");
            modelBuilder.Entity<Contact>()
                .Property(e => e.DeletionTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default","(getutcdate())");
            modelBuilder.Entity<Contact>()
                .Property(e => e.ModificationTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Contact>()
                .Property(e => e.ModificationUserId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Contact>()
                .Property(e => e.CreatorUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Contact>()
                .Property(e => e.DeletionUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Company_Registration>()
                 .HasKey(x => x.Id);
            modelBuilder.Entity<Company_Registration>()
                 .Property(e => e.IsDeleted)
                    .HasColumnType("bit")
                    .HasColumnAnnotation("Default", "0"); 
            modelBuilder.Entity<Company_Registration>()
                .Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Company_Registration>()
                .Property(e => e.DeletionTime)
                   .HasColumnType("datetime")
                  .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Company_Registration>()
                .Property(e => e.ModificationTime)
                   .HasColumnType("datetime")
                  .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Company_Registration>()
                .Property(e => e.ModificationUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Company_Registration>()
                .Property(e => e.CreatorUserId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Company_Registration>()
                .Property(e => e.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Company_Registration>()
                .Property(e => e.PackageId);
            modelBuilder.Entity<Company_Registration>()
                .Property(e => e.DeletionUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Company_Officers>()
                .Property(e => e.IsDeleted)
                    .HasColumnType("bit")
                    .HasColumnAnnotation("Default", "0");
            modelBuilder.Entity<Company_Officers>()
                .Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Company_Officers>()
                .Property(e => e.DeletionTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Company_Officers>()
                .Property(e => e.ModificationTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Company_Officers>()
                .Property(e => e.ModificationUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Company_Officers>()
                .Property(e => e.CreatorUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Company_Officers>()
                .Property(e => e.DeletionUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<AddOnService>()
                .Property(e => e.ServiceName)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<AddOnService>()
                .Property(e => e.ServiceName)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<AddOnService>()
                .Property(e => e.IsDeleted)
                    .HasColumnType("bit")
                    .HasColumnAnnotation("Default", "0");
            modelBuilder.Entity<AddOnService>()
                .Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<AddOnService>()
                .Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<AddOnService>()
                .Property(e => e.DeletionTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<AddOnService>()
                .Property(e => e.DeletionTime)
                   .HasColumnType("datetime")
                  .HasColumnAnnotation("Default", "(getutcdate())");

            modelBuilder.Entity<AddOnService>()
                .Property(e => e.ModificationTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<AddOnService>()
                .Property(e => e.CreatorUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<AddOnService>()
                .Property(e => e.ModificationUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<AddOnService>()
                .Property(e => e.DeletionUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Payments>()
                .Property(e => e.IsDeleted)
                    .HasColumnType("bit")
                    .HasColumnAnnotation("Default", "0");
            modelBuilder.Entity<Payments>()
                .Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Payments>()
                .Property(e => e.DeletionTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Payments>()
                .Property(e => e.ModificationTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Payments>()
                .Property(e => e.ModificationUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Payments>()
                .Property(e => e.CreatorUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Payments>()
                .Property(e => e.DeletionUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                  .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Package>()
                .Property(e => e.IsDeleted)
                    .HasColumnType("bit")
                    .HasColumnAnnotation("Default", "0");
            modelBuilder.Entity<Package>()
                .Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Package>()
                .Property(e => e.DeletionTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Package>()
                .Property(e => e.ModificationTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Package>()
                .Property(e => e.ModificationUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Package>()
                .Property(e => e.CreatorUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Package>()
                .Property(e => e.DeletionUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Incentives>()
                .Property(e => e.IsDeleted)
                    .HasColumnType("bit")
                    .HasColumnAnnotation("Default", "0");
            modelBuilder.Entity<Incentives>()
                .Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Incentives>()
                .Property(e => e.DeletionTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Incentives>()
                .Property(e => e.ModificationTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Incentives>()
                .Property(e => e.ModificationUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Incentives>()
                .Property(e => e.CreatorUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Incentives>()
                .Property(e => e.DeletionUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Comp_Incentives>()
                .Property(e => e.IsDeleted)
                    .HasColumnType("bit")
                    .HasColumnAnnotation("Default", "0");
            modelBuilder.Entity<Comp_Incentives>()
                .Property(e => e.IsDeleted)
                    .HasColumnType("bit")
                    .HasColumnAnnotation("Default", "0");
            modelBuilder.Entity<Comp_Incentives>()
                .Property(e => e.IsDeleted)
                    .HasColumnType("bit")
                    .HasColumnAnnotation("Default", "0");
            modelBuilder.Entity<Comp_Incentives>()
                .Property(e => e.IsDeleted)
                    .HasColumnType("bit")
                    .HasColumnAnnotation("Default", "0");
            modelBuilder.Entity<Comp_Incentives>()
                .Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Comp_Incentives>()
                .Property(e => e.DeletionTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Comp_Incentives>()
                .Property(e => e.ModificationTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<Comp_Incentives>()
                .Property(e => e.ModificationUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Comp_Incentives>()
                .Property(e => e.CreatorUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Comp_Incentives>()
                .Property(e => e.DeletionUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<ChatHeader>()
                .Property(e => e.IsDeleted)
                    .HasColumnType("bit")
                    .HasColumnAnnotation("Default", "0");
            modelBuilder.Entity<ChatHeader>()
                .Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<ChatHeader>()
                .Property(e => e.DeletionTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<ChatHeader>()
                .Property(e => e.ModificationTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<ChatHeader>()
                .Property(e => e.ModificationUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<ChatHeader>()
                .Property(e => e.CreatorUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<ChatHeader>()
                .Property(e => e.DeletionUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<ChatThread>()
                .Property(e => e.IsDeleted)
                    .HasColumnType("bit")
                    .HasColumnAnnotation("Default", "0");
            modelBuilder.Entity<ChatThread>()
                .Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<ChatThread>()
                .Property(e => e.DeletionTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<ChatThread>()
                .Property(e => e.ModificationTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default", "(getutcdate())");
            modelBuilder.Entity<ChatThread>()
                .Property(e => e.ModificationUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                   .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<ChatThread>()
                .Property(e => e.CreatorUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<ChatThread>()
                .Property(e => e.DeletionUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default", "('')");
            modelBuilder.Entity<Settings>()
                .Property(e => e.IsDeleted)
                    .HasColumnType("bit")
                    .HasColumnAnnotation("Default","0");
            modelBuilder.Entity<Settings>()
                .Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasColumnAnnotation("Default","(getutcdate())");
            modelBuilder.Entity<Settings>()
                .Property(e => e.DeletionTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default","(getutcdate())");
            modelBuilder.Entity<Settings>()
                .Property(e => e.ModificationTime)
                   .HasColumnType("datetime")
                   .HasColumnAnnotation("Default","(getutcdate())");
            modelBuilder.Entity<Settings>()
                .Property(e => e.ModificationUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default","('')");
            modelBuilder.Entity<Settings>()
                .Property(e => e.CreatorUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default","('')");
            modelBuilder.Entity<Settings>()
                .Property(e => e.DeletionUserId)
                    
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnAnnotation("Default","('')");
            modelBuilder.Entity<Settings>();
        }

    }
}


