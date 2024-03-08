
namespace FazUmPix.Models;

public class User {

    public required string Id { get; set; }
    public required string CPF { get; set; }
    public required string Name { get; set; }
    
    public List<PaymentProviderAccount>? Accounts { get; }
    public User(string cpf, string name) {
        this.CPF = cpf;
        this.Name = name;
    }
}