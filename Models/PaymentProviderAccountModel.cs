
namespace FazUmPix.Models;

public class PaymentProviderAccount
{

    public required uint Id { get; set; }
    public required int Agency { get; set; }
    public required int AgencyVerify { get; set; }
    public required uint Number { get; set; }
    public required uint NumberVerify { get; set; }

    public required User User { get; set; }
    public required uint UserId { get; set; }
    public required PaymentProvider PaymentProvider { get; set; }
    public required uint PaymentProviderId { get; set; }
    public List<PixKey>? PixKeys { get; }
}