
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FazUmPix.Models;

public class Payment : BaseModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public required string Status { get; set; }
    public required long Amount { get; set; }
    public string? Description { get; set; }


    public required uint OriginPaymentProviderAccountId { get; set; }
    public required PaymentProviderAccount OriginPaymentProviderAccount { get; set; }

    public required uint DestinationPaymentProviderAccountId { get; set; }
    public required PaymentProviderAccount DestinationPaymentProviderAccount { get; set; }

    public required uint PixKeyId { get; set; }
    public required PixKey PixKey { get; set; }
}