namespace OggettoCase.DataAccess.Context;

public interface IDatabaseContextFactory
{
    DatabaseContext CreateDbContext();
}
