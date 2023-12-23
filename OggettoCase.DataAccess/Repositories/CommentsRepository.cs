using Microsoft.EntityFrameworkCore;
using OggettoCase.DataAccess.Context;
using OggettoCase.DataAccess.Interfaces;
using OggettoCase.DataAccess.Models.Comments;

namespace OggettoCase.DataAccess.Repositories;

public class CommentsRepository: BaseRepository, ICommentsRepository
{
    
    public CommentsRepository(IDatabaseContextFactory contextFactory) : base(contextFactory)
    {
    }
    
    public async Task<Comment> CreateCommentAsync(Guid calenderEventId, long userId, string message, CancellationToken ct = default)
    {
        var context = ContextFactory.CreateDbContext();
        var comment = new Comment
        {
            Text = message,
            CalendarId = calenderEventId,
            UserId = userId,
        };

        var result = await context.Comments.AddAsync(comment, ct);
        await context.SaveChangesAsync(ct);

        return result.Entity;
    }

    public async Task<IEnumerable<Comment>> GetAllAsync(Guid calenderEventId, CancellationToken ct = default)
    {
        var context = ContextFactory.CreateDbContext();
        var result = await context.Comments.Where(c => c.CalendarId == calenderEventId).ToListAsync(ct);

        return result;
    }

    public Task<IEnumerable<Comment>> GetAllAsync(CancellationToken ct = default)
    {
        throw new NotSupportedException();
    }

    public Task<Comment> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotSupportedException();
    }

    public Task<Comment> UpdateAsync(Comment entityToUpdate, CancellationToken ct = default)
    {
        throw new NotSupportedException();
    }

    public Task<Guid> DeleteByIdAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotSupportedException();
    }
}