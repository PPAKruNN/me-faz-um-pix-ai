
using FazUmPix.DTOs;
using FazUmPix.Models;
using FazUmPix.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FazUmPix.Controllers;

[ApiController]
[Route("[controller]")]
public class KeysController(KeysService keysService) : ControllerBase
{
    private readonly KeysService _keysService = keysService;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateKeyInputDTO dto)
    {
        PaymentProvider paymentProvider = AuthReader.GetPaymentProvider(HttpContext);
        CreateKeyOutputDTO key = await _keysService.CreateKey(dto, paymentProvider);

        return CreatedAtAction(null, null, key);
    }

    [HttpGet("/Keys/{Type}/{Value}")]
    [Authorize]
    public async Task<IActionResult> Read([FromRoute] ReadKeyInputDTO dto)
    {
        TokenDTO token = AuthReader.GetToken(HttpContext);
        ReadKeyOutputDTO key = await _keysService.ReadKey(dto, token);

        return Ok(key);
    }

}
