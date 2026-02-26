using Microsoft.EntityFrameworkCore;
using TaskCore.Models;

namespace TaskCore.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("Users");
            e.HasKey(u => u.Id);
            e.Property(u => u.Name).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<TaskItem>(e =>
        {
            e.ToTable("Tasks");
            e.HasKey(t => t.Id);
            e.Property(t => t.CurrentStatus).HasDefaultValue(1);
            e.Property(t => t.IsOpen).HasDefaultValue(true);
            e.Property(t => t.CustomData).HasColumnType("nvarchar(max)");

            e.HasOne(t => t.AssignedUser)
             .WithMany(u => u.Tasks)
             .HasForeignKey(t => t.AssignedUserId);
        });

    }
}
