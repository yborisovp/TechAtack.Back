using Microsoft.EntityFrameworkCore;
using OggettoCase.DataAccess.Models.Users;
using Toolbelt.ComponentModel.DataAnnotations;

namespace OggettoCase.DataAccess.Context;

public class DatabaseContext : DbContext
{
    public const string DefaultSchema = "oggetto_case";
    public const string DefaultMigrationHistoryTableName = "__MigrationsHistory";
    
    public DbSet<User> Users { get; set; } = null!;

    public DatabaseContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DefaultSchema);
        modelBuilder.BuildIndexesFromAnnotations();
    }
}