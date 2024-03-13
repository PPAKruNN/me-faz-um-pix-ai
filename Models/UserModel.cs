
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FazUmPix.Models;

public class User : BaseModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public required string CPF { get; set; }
    public required string Name { get; set; }
    public List<PaymentProviderAccount>? Accounts { get; }
}