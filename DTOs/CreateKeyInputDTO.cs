
using System.ComponentModel.DataAnnotations;

namespace FazUmPix.DTOs;

public class CreateKeyInputDTO
{
    [Required]
    public required PixKeyDTO Key { get; set; }
    [Required]
    public required UserCPFDTO User { get; set; }
    [Required]
    public required AccountDTO Account { get; set; }

}