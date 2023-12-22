using System.ComponentModel.DataAnnotations;
using ServiceTemplate.DataContracts.Dtos.Templates.Enums;

namespace ServiceTemplate.DataContracts.Dtos.Templates;

public class UpdateTemplateDto
{
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public TemplateEnumDto TemplateEnum { get; set; } = TemplateEnumDto.FirstEnumAttribute;
}