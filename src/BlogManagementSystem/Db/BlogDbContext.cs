using System;
using BlogManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogManagementSystem.Db
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Comment>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Post>().Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Post>().Property(x => x.Date)
                .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Post>().Property(x => x.Date)
                .HasDefaultValue(DateTime.UtcNow);

            modelBuilder.Entity<Comment>().Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Comment>().Property(x => x.Date)
                .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Comment>().Property(x => x.Date)
                .HasDefaultValue(DateTime.UtcNow);

            modelBuilder.Entity<Post>()
                .HasMany(x => x.Comments)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.PostId);
        }
    }
}