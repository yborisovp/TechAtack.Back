using OggettoCase.DataAccess.Models.Comments;
using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataContracts.Dtos.Comments;
using OggettoCase.DataContracts.Dtos.Users;
using OggettoCase.Mappers.Users;

namespace OggettoCase.Mappers.Comments;

/// <summary>
/// Templates Mapper from dto to db entity model
/// </summary>
public static class DbToDtoCommentMapper
{
    public static CommentDto ToDto(this Comment comment)
    {
        return new CommentDto
        {
            Id = comment.Id,
            CalendarId = comment.CalendarId,
            Text = comment.Text,
            User = comment.User.ToDto(),
            CreatedAt = comment.CreatedAt
        };
    }
}