using Microsoft.EntityFrameworkCore;
using OggettoCase.DataAccess.Models.Calendars;
using OggettoCase.DataAccess.Models.Categories;
using OggettoCase.DataAccess.Models.Comments;
using OggettoCase.DataAccess.Models.Users;
using Toolbelt.ComponentModel.DataAnnotations;

namespace OggettoCase.DataAccess.Context;

public class DatabaseContext : DbContext
{
    public const string DefaultSchema = "oggetto_case";
    public const string DefaultMigrationHistoryTableName = "__MigrationsHistory";
    
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Calendar> Calendars { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    public DatabaseContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DefaultSchema);
        modelBuilder.BuildIndexesFromAnnotations();

        modelBuilder.Entity<Calendar>().HasOne(x => x.Owner).WithMany(x => x.CalendarEvents)
            .HasForeignKey(x => x.OwnerId).HasConstraintName("owner_id").OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>().HasMany(x => x.CalendarEvents).WithOne(x => x.Owner)
            .OnDelete(DeleteBehavior.Cascade);
           
        modelBuilder.Entity<Comment>().HasOne(x => x.Calendar).WithMany(x => x.Comments)
            .HasForeignKey(x => x.CalendarId).HasConstraintName("calendar_event_id");
    }
}