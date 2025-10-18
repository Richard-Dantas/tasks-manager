using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Tasks.Manager.Domain.Entities;

namespace Tasks.Manager.Infrastructure.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        try 
        { 
            var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (databaseCreator != null) 
            {
                if (!databaseCreator.CanConnect()) databaseCreator.Create();
                if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
            }
        } 
        catch (Exception ex)
        { 
            Console.WriteLine(ex.Message);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProjectUser>()
            .HasKey(pu => new { pu.ProjectId, pu.UserId });

        modelBuilder.Entity<ProjectUser>()
            .HasOne(pu => pu.Project)
            .WithMany(p => p.Members)
            .HasForeignKey(pu => pu.ProjectId);

        modelBuilder.Entity<ProjectUser>()
            .HasOne(pu => pu.User)
            .WithMany()
            .HasForeignKey(pu => pu.UserId);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Project>()
            .Navigation(p => p.Tasks)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        modelBuilder.Entity<TaskItem>()
            .Property(t => t.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<TaskItem>()
            .HasMany(t => t.History)
            .WithOne()
            .HasForeignKey(h => h.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TaskItem>()
            .HasMany(t => t.Comments)
            .WithOne()
            .HasForeignKey(c => c.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TaskItem>()
            .Navigation(t => t.History)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        modelBuilder.Entity<TaskItem>()
            .Navigation(t => t.Comments)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        modelBuilder.Entity<TaskHistory>()
            .Property(t => t.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<TaskComment>()
            .Property(t => t.Id)
            .ValueGeneratedNever();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectUser> ProjectUsers { get; set; }
    public DbSet<TaskHistory> TaskHistories { get; set; }

}
