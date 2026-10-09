using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class FundooContext : DbContext
    {
        public FundooContext(DbContextOptions<FundooContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users => Set<UserEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(user =>
            {
                user.ToTable("Users");
                user.HasKey(u => u.UserId);

                user.Property(u => u.FirstName).HasMaxLength(50).IsRequired();
                user.Property(u => u.LastName).HasMaxLength(50).IsRequired();
                user.Property(u => u.Email).HasMaxLength(100).IsRequired();
                user.Property(u => u.PasswordHash).IsRequired();
                user.Property(u => u.ContactNo).HasMaxLength(10).IsRequired();

                // Two users can't share an email; also makes lookups by email fast.
                user.HasIndex(u => u.Email).IsUnique();
                user.HasIndex(u => u.ResetTokenHash);
            });
        }
    }
}
