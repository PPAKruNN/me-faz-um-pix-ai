
namespace FazUmPix.DTOs;

public class CreatePaymentInputDTO
{
    public required OriginDTO Origin { get; set; }
    public required DestinyDTO Destiny { get; set; }
    public required long Amount { get; set; }
    public string? Description { get; set; }
}
