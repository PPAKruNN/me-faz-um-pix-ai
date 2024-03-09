
using System.ComponentModel.DataAnnotations;

namespace FazUmPix.DTOs
{
    public class CreateKeyOutputDTO
    {
        [Required]
        [RegularExpression("Random|CPF|Email|Phone")]
        public required string Type { get; set; }
        [Required]
        public required string Value { get; set; }
    }
}
