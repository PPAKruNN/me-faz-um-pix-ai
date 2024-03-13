
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FazUmPix.Models;

public class PaymentProvider : BaseModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public required string Name { get; set; }
    public required Guid Token { get; set; }
    
    public List<PaymentProviderAccount>? Accounts { get; }
}
