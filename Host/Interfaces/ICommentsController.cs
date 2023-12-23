using OggettoCase.DataContracts.Dtos.Comments;

namespace OggettoCase.Interfaces;

public interface ICommentsController: IBaseController<CommentDto, Guid, CommentDto>
{
    public Task<IEnumerable<CommentDto>> GetAllAsync(Guid calenderEventId, CancellationToken ct = default);
}