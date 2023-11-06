using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.DBContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .HasMany(u => u.Tags)
            .WithMany(t => t.Users)
            .UsingEntity<TagToUser>(
                e => e
                    .HasOne(ttu => ttu.Tag)
                    .WithMany(t => t.TagsToUsers)
                    .HasForeignKey(ttu => ttu.TagId),
                e => e
                    .HasOne(ttu => ttu.User)
                    .WithMany(u => u.TagsToUsers)
                    .HasForeignKey(ttu => ttu.UserId),
                e =>
                {
                    e.HasKey(ttu => new { ttu.UserId, ttu.TagId });
                    e.ToTable("TagToUser");
                });
        
        modelBuilder
            .Entity<TagToUser>()
            .HasKey(ttu => ttu.EntityId);

        modelBuilder
            .Entity<TagToUser>()
            .Property(ttu => ttu.EntityId)
            .ValueGeneratedOnAdd();
    }
}