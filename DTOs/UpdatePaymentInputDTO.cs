
using System.ComponentModel.DataAnnotations;

public class UpdatePaymentInputDTO
{
    [RegularExpression(@"^(?:ACCEPTED|REJECTED)$")]
    public string Status { get; set; }
}