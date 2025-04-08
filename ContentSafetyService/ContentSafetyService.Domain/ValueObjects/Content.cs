using System.ComponentModel.DataAnnotations;

namespace ContentSafetyService.Domain.ValueObjects;

public record Content()
{
    [Required]
    [StringLength(1000, MinimumLength = 10)]
    public string Text { get; set; }
}