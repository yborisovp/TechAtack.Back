using OggettoCase.DataContracts.Dtos.Comments;

namespace OggettoCase.DataContracts.Interfaces;

public interface ICommentsService: IBaseService<CommentDto, Guid, CommentDto>
{
    public Task<CommentDto> CreateCommentAsync(Guid calenderEventId, long userId, string message, CancellationToken ct = default);

    public Task<IEnumerable<CommentDto>> GetAllAsync(Guid calenderEventId, CancellationToken ct = default);
}