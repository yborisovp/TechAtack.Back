using System.ComponentModel.DataAnnotations;

namespace OggettoCase.DataContracts.Dtos.Category;

public class CategoryDto
{
    public int Id { get; set; }
    [MaxLength(100)]
    public required string Description { get; set; }
}