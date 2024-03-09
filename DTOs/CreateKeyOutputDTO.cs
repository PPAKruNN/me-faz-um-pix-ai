
using System.ComponentModel.DataAnnotations;

namespace FazUmPix.DTOs
{
    public class CreateKeyOutputDTO
    {
        [Required]
        [RegularExpression("Random|CPF|Email|Phone")]
        public string Type { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
