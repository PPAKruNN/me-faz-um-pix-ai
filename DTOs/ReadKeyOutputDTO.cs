using System.ComponentModel.DataAnnotations;

namespace FazUmPix.DTOs;

public class ReadKeyOutputDTO
{
    [Required]
    public required PixKeyDTO Key { get; set; }
    [Required]
    public required MaskedUserDTO User { get; set; }
    [Required]
    public required CompleteAccountDTO Account { get; set; }
}
