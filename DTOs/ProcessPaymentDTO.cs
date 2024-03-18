namespace FazUmPix.DTOs;

public class ProcessPaymentDTO
{
    public required string ProcessURL { get; set; }
    public required string AcknowledgeURL { get; set; }
    public required uint PaymentId { get; set; }
    public required CreatePaymentInputDTO Data { get; set; }
}