
using OggettoCase.DataAccess.Models.Categories;
using OggettoCase.DataContracts.Dtos.Category;

namespace OggettoCase.Mappers.Categories;

/// <summary>
/// Templates Mapper from dto to db entity model
/// </summary>
public static class DbToDtoCategoryMapper
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Description = category.Description
        };
    }
}