using System.ComponentModel.DataAnnotations;

namespace FazUmPix.DTOs;

public class PixKeyDTO
{
    [Required]
    [RegularExpression("Random|CPF|Phone|Email")]
    public required string Type { get; set; }
    [Required]
    public required string Value { get; set; }

}