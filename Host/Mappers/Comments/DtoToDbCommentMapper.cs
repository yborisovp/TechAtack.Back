using OggettoCase.DataAccess.Models.Comments;
using OggettoCase.DataContracts.Dtos.Comments;

namespace OggettoCase.Mappers.Comments;

/// <summary>
/// Templates Mapper fromm db entity model to dto
/// </summary>
public static class DtoToDbCommentMapper
{
    public static Comment ToEntity(this CommentDto commentDto)
    {
        return new Comment
        {
            Id = commentDto.Id,
            Text = commentDto.Text,
            CalendarId = commentDto.CalendarId,
            UserId = commentDto.User.Id
        };
    }

}