using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KrisKringle.Models
{
    public partial class Kris_dbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public Kris_dbContext()
        {
        }

        public Kris_dbContext(DbContextOptions<Kris_dbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Footprints> Footprints { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Wish> Wish { get; set; }
        public virtual DbSet<WishDates> WishDates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                        .UseLazyLoadingProxies()
                        .UseSqlServer(_configuration.GetConnectionString("Kris_dbContext"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Comment1).HasColumnName("Comment");

                entity.Property(e => e.CommentDeletedTime)
                    .HasColumnName("Comment_Deleted_Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.CommentTime)
                    .HasColumnName("Comment_Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserComment)
                    .HasColumnName("User_Comment")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Footprints>(entity =>
            {
                entity.Property(e => e.LogTime)
                    .HasColumnName("Log_Time")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.Image1).HasColumnName("Image");

                entity.Property(e => e.ImageDeleted)
                    .HasColumnName("Image_Deleted")
                    .HasColumnType("datetime");

                entity.Property(e => e.ImagePath).HasColumnName("Image_Path");

                entity.Property(e => e.ImageTime)
                    .HasColumnName("Image_Time")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Nick).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(10);

                entity.Property(e => e.Username).HasMaxLength(10);
            });

            modelBuilder.Entity<Wish>(entity =>
            {
                entity.Property(e => e.Wish1).HasColumnName("Wish");

                entity.Property(e => e.WishDate)
                    .HasColumnName("Wish_Date")
                    .HasMaxLength(50);

                entity.Property(e => e.WishTime)
                    .HasColumnName("Wish_Time")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<WishDates>(entity =>
            {
                entity.Property(e => e.Dates).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
