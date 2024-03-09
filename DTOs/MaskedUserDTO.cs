using System.ComponentModel.DataAnnotations;

namespace FazUmPix.DTOs;

public class MaskedUserDTO
{
    [Required]
    public required string Name { get; set; }

    [Required]
    public required string MaskedCpf { get; set; }

}
