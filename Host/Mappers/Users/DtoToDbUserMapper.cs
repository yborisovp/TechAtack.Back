using OggettoCase.DataAccess.Dtos;
using OggettoCase.DataAccess.Models.Comments;
using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataContracts.Dtos.Comments;
using OggettoCase.DataContracts.Dtos.Users;

namespace OggettoCase.Mappers.Users;

/// <summary>
/// Templates Mapper fromm db entity model to dto
/// </summary>
public static class DtoToDbUserMapper
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