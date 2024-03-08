
namespace FazUmPix.Models;

public class PaymentProvider {
    public required uint Id { get; set; }
    public required string Name { get; set; }
    public required string Token { get; set; }
    
    public List<PaymentProviderAccount>? Accounts { get; }
}
