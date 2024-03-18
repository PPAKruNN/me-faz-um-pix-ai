
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FazUmPix.Models;

public class PixKey : BaseModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public required string Type { get; set; }
    public required string Value { get; set; }

    public required uint PaymentProviderAccountId { get; set; }
    public required PaymentProviderAccount Account { get; set; }
    public List<Payment>? Payments { get; }
}