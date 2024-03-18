
using System.Security.Claims;
using FazUmPix.DTOs;
using FazUmPix.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FazUmPix.Controllers;

[ApiController]
[Route("[controller]")]
public class KeysController(KeysService keysService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateKeyInputDTO dto)
    {
        Guid tokenStr = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Authentication).Value);
        string paymentProviderSerialized = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.UserData).Value;

        TokenDTO token = new TokenDTO { Token = tokenStr };

        CreateKeyOutputDTO key = await keysService.CreateKey(dto, token, paymentProviderSerialized);

        return CreatedAtAction(null, null, key);
    }

    [HttpGet("/Keys/{Type}/{Value}")]
    [Authorize]
    public async Task<IActionResult> Read([FromRoute] ReadKeyInputDTO dto)
    {
        Guid tokenStr = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Authentication).Value);
        string paymentProviderSerialized = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.UserData).Value;

        TokenDTO token = new TokenDTO { Token = tokenStr };

        ReadKeyOutputDTO key = await keysService.ReadKey(dto, token, paymentProviderSerialized);

        return Ok(key);
    }
}
