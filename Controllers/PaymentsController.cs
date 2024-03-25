using FazUmPix.DTOs;
using FazUmPix.Models;
using FazUmPix.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FazUmPix.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentsController(PaymentsService paymentsService) : ControllerBase
{
    private readonly PaymentsService _paymentsService = paymentsService;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Payment([FromBody] CreatePaymentInputDTO dto)
    {
        var paymentProvider = AuthReader.GetPaymentProvider(HttpContext);

        CreatePaymentOutputDTO response =
            await _paymentsService.CreatePayment(dto, paymentProvider);

        return CreatedAtAction(null, null, response);
    }

    [HttpPatch]
    [Route("{Id}")]
    public async Task<IActionResult> UpdatePayment([FromRoute] RouteIdDTO idDto, [FromBody] UpdatePaymentInputDTO dto)
    {
        await _paymentsService.UpdatePayment(dto, idDto);

        return NoContent();
    }

    [HttpPost]
    [Route("Concilliation")]
    public async Task<IActionResult> Concilliation([FromBody] ConcilliationInputDTO dto)
    {
        var paymentProvider = AuthReader.GetPaymentProvider(HttpContext);

        await _paymentsService.CreatePaymentsConcilliation(dto, paymentProvider);

        return NoContent();
    }

}