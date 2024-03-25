using FazUmPix.Models;

namespace FazUmPix.DTOs;

public class CreatePaymentOutputDTO : BaseModel
{
    public required uint Id { get; set; }
    public required string Status { get; set; }
    public required long Amount { get; set; }
    public string? Description { get; set; }
    public required DestinationPaymentProviderAccountDTO Destination { get; set; }
    public required PixKeyDTO PixKey { get; set; }
}