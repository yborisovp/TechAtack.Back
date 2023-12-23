using OggettoCase.DataAccess.Interfaces;
using OggettoCase.DataContracts.Dtos.Comments;
using OggettoCase.DataContracts.Interfaces;
using OggettoCase.Mappers.Comments;

namespace OggettoCase.Services;

public class CommentsService: ICommentsService
{
    private readonly ICommentsRepository _commentsRepository;

    public CommentsService(ICommentsRepository commentsRepository)
    {
        _commentsRepository = commentsRepository;
    }

    public async Task<CommentDto> CreateCommentAsync(Guid calenderEventId, long userId, string message, CancellationToken ct = default)
    {
        var comment = await _commentsRepository.CreateCommentAsync(calenderEventId, userId, message, ct);
        return comment.ToDto();
    }

    public async Task<IEnumerable<CommentDto>> GetAllAsync(Guid calenderEventId, CancellationToken ct = default)
    {
        var comment = await _commentsRepository.GetAllAsync(calenderEventId, ct);
        return comment.Select(c => c.ToDto());
    }

    public Task<IEnumerable<CommentDto>> GetAllAsync(CancellationToken ct = default)
    {
        throw new NotSupportedException();
    }

    public Task<CommentDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotSupportedException();
    }

    public Task<CommentDto> UpdateByIdAsync(Guid id, CommentDto dtoToUpdate, CancellationToken ct = default)
    {
        throw new NotSupportedException();
    }

    public Task<Guid> DeleteByIdAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotSupportedException();
    }
}