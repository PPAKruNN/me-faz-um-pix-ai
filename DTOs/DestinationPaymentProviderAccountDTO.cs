namespace FazUmPix.DTOs;

public class DestinationPaymentProviderAccountDTO
{
    public required int Agency { get; set; }
    public required int Number { get; set; }
    public required Guid PaymentProviderToken { get; set; }
}