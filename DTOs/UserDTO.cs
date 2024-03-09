
using System.ComponentModel.DataAnnotations;

namespace FazUmPix.DTOs;

public class UserCPFDTO 
{
    [Required]
    [StringLength(11)]
    public required string CPF { get; set; }
}