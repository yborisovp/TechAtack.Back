using OggettoCase.DataAccess.Context;

namespace OggettoCase.DataAccess.Repositories
{
    public class BaseRepository
    {
        protected IDatabaseContextFactory ContextFactory { get; set; }

        protected BaseRepository(IDatabaseContextFactory contextFactory)
        {
            ContextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }
    }
}
