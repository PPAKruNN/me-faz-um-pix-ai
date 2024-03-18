
using System.ComponentModel.DataAnnotations;

namespace FazUmPix.DTOs;
public class AccountDTO
{

    [Required]
    public required string Number { get; set; }
    [Required]
    public required string Agency { get; set; }

}