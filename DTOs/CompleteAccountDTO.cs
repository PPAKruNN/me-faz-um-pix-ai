using System.ComponentModel.DataAnnotations;

namespace FazUmPix.DTOs;

public class CompleteAccountDTO
{
    [Required]
    public required string Number { get; set; }
    [Required]
    public required string Agency { get; set; }
    [Required]
    public required string BankName { get; set; }
    [Required]
    public required string BankId { get; set; }
}
