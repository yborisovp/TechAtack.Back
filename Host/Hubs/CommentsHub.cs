using Microsoft.AspNetCore.SignalR;
using OggettoCase.DataContracts.Interfaces;

namespace OggettoCase.Hubs;

public class CommentsHub: Hub
{
    private ICommentsService _commentsService;

    public CommentsHub(ICommentsService commentsService)
    {
        _commentsService = commentsService;
    }

    public async Task SendMessage(Guid calenderEventId, long userId, string message, CancellationToken ct = default)
    {
        var comment = await _commentsService.CreateCommentAsync(calenderEventId, userId, message, ct);
        await Clients.All.SendAsync("ReceiveMessage", comment, ct);
    }
}