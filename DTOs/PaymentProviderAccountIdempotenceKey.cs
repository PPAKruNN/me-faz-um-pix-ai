
namespace FazUmPix.DTOs;

public class PaymentProviderAccountIdempotenceKey
{
    public required string Agency { get; set; }
    public required string Number { get; set; }

}