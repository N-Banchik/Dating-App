using Microsoft.EntityFrameworkCore;
using API.Entities;
namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.TargetUserId });
            modelBuilder.Entity<UserLike>()
                .HasOne(u => u.SourceUser)
                .WithMany(u => u.LikedUsers)
                .HasForeignKey(u => u.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserLike>().HasOne(u => u.TargetUser)
                .WithMany(u => u.LikedByUsers)
                .HasForeignKey(u => u.TargetUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> likes { get; set; }





    }
}