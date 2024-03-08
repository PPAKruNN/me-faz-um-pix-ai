
namespace FazUmPix.Models;

public class PixKey
{

    public required uint Id { get; set; }
    public required string Type { get; set; }
    public required string Value { get; set; }

    public required uint PaymentProviderAccountId { get; set; }
    public required PaymentProviderAccount Account { get; set; }
}