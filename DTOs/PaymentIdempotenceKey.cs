namespace FazUmPix.DTOs;

public class PaymentIdempotenceKey
{
    public required string PixKeyValue { get; set; }
    public required long Amount { get; set; }
    public required PaymentProviderAccountIdempotenceKey Account { get; set; }
}