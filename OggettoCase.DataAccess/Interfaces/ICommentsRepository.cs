using OggettoCase.DataAccess.Models.Comments;

namespace OggettoCase.DataAccess.Interfaces;

public interface ICommentsRepository: IRepository<Comment, Guid>
{
    public Task<Comment> CreateCommentAsync(Guid calenderEventId, long userId, string message, CancellationToken ct = default);
    public Task<IEnumerable<Comment>> GetAllAsync(Guid calenderEventId, CancellationToken ct = default);
}