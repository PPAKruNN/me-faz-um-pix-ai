namespace FazUmPix.Controllers;

using System.Security.Claims;
using FazUmPix.DTOs;
using FazUmPix.Models;
using FazUmPix.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class PaymentsController(PaymentsService paymentsService) : ControllerBase
{
    public readonly PaymentsService _paymentsService = paymentsService;


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Payment([FromBody] CreatePaymentInputDTO dto)
    {
        Guid tokenStr = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Authentication).Value);
        string paymentProviderSerialized = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.UserData).Value;

        TokenDTO tokenDTO = new()
        {
            Token = tokenStr
        };

        Payment response = await _paymentsService.CreatePayment(dto, tokenDTO, paymentProviderSerialized);
        CreatePaymentOutputDTO outputDTO = new()
        {
            Id = response.Id,
            Amount = response.Amount,
            Description = response.Description,
            Destination = new DestinationPaymentProviderAccountDTO
            {
                Agency = response.DestinationPaymentProviderAccount.Agency,
                Number = response.DestinationPaymentProviderAccount.Number,
                PaymentProviderToken = tokenDTO.Token
            },
            PixKey = new PixKeyDTO
            {
                Value = response.PixKey.Value,
                Type = response.PixKey.Type
            },
            Status = response.Status,
            CreatedAt = response.CreatedAt,
            UpdatedAt = response.UpdatedAt
        };

        return CreatedAtAction(null, null, outputDTO);
    }

    [HttpPatch]
    [Route("{Id}")]
    public async Task<IActionResult> UpdatePayment([FromRoute] RouteIdDTO idDTO, [FromBody] UpdatePaymentInputDTO dto)
    {
        await _paymentsService.UpdatePayment(dto, idDTO);

        return NoContent();
    }

}