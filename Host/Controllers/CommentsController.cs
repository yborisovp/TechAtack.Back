using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OggettoCase.DataContracts.Dtos.Comments;
using OggettoCase.DataContracts.Interfaces;
using OggettoCase.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace OggettoCase.Controllers;

[ApiController]
[Authorize(Policy = "ExcludeRoles")]
[Route("[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class CommentsController: ICommentsController
{

    private readonly ICommentsService _commentsService;

    /// <summary>
    /// Constructor of CommentsController
    /// </summary>
    /// <param name="commentsService"></param>
    public CommentsController(ICommentsService commentsService)
    {
        _commentsService = commentsService;
    }
    
    [AllowAnonymous]
    [HttpGet]
    [SwaggerOperation($"Get all {nameof(CommentDto)}s")]
    [SwaggerResponse(200, type: typeof(IEnumerable<CommentDto>), description: $"List of {nameof(CommentDto)}s")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]

    public async Task<IEnumerable<CommentDto>> GetAllAsync(Guid calenderEventId, CancellationToken ct = default)
    {
        return await _commentsService.GetAllAsync(calenderEventId, ct);
    }
    
    public Task<ActionResult<IEnumerable<CommentDto>>> GetAllAsync(CancellationToken ct = default)
    {
        throw new NotSupportedException();
    }

    public Task<ActionResult<CommentDto>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotSupportedException();
    }

    public Task<ActionResult<CommentDto>> UpdateByIdAsync(Guid id, CommentDto dtoToUpdate, CancellationToken ct = default)
    {
        throw new NotSupportedException();
    }

    public Task<ActionResult<Guid>> DeleteByIdAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotSupportedException();
    }
}