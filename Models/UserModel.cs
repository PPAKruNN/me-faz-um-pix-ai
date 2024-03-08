
namespace FazUmPix.Models;

public class User {

    public required uint Id { get; set; }
    public required string CPF { get; set; }
    public required string Name { get; set; }
    
    public List<PaymentProviderAccount>? Accounts { get; }
}