using OggettoCase.DataAccess.Models.Categories;
using OggettoCase.DataContracts.Dtos.Category;

namespace OggettoCase.Mappers.Categories;

/// <summary>
/// Templates Mapper fromm db entity model to dto
/// </summary>
public static class DtoToDbCategoryMapper
{
    public static Category ToEntity(this CategoryDto category)
    {
        return new Category
        {
            Id = category.Id,
            Description = category.Description
        };
    }

}