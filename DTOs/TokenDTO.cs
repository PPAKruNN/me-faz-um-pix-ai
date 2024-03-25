
using System.ComponentModel.DataAnnotations;

namespace FazUmPix.DTOs;

public class TokenDTO
{

    [Required]
    public required Guid Token { get; set; }
}
