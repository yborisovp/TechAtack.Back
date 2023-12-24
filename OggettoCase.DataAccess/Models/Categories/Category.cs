using System.ComponentModel.DataAnnotations;

namespace OggettoCase.DataAccess.Models.Categories;

public class Category
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string Description { get; set; }
}