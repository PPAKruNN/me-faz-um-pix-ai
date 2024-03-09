
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FazUmPix.Models;

public class PaymentProviderAccount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public required int Agency { get; set; }
    public required int Number { get; set; }

    public required uint UserId { get; set; }
    public required uint PaymentProviderId { get; set; }

    public required User User { get; set; }
    public required PaymentProvider Bank { get; set; }
    public List<PixKey>? PixKeys { get; }
}