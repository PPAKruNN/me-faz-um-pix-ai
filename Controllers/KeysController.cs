
using FazUmPix.DTOs;
using FazUmPix.Models;
using FazUmPix.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FazUmPix.Controllers;

[ApiController]
[Route("[controller]")]
public class KeysController(KeysService keysService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromHeader] string Authorization, [FromBody] CreateKeyInputDTO dto)
    {
        TokenDTO token;
        if (Authorization == null) return Unauthorized("No authorization header");
        else
        {
            try
            {
                var TokenString = Authorization.Split(" ")[1];
                token = new TokenDTO { Token = Guid.Parse(TokenString) };
            }
            catch (Exception)
            {
                return UnprocessableEntity("Invalid GUID format");
            }
        }

        CreateKeyOutputDTO key = await keysService.CreateKey(dto, token);

        return Ok(key);
    }

    [HttpGet("/Keys/{Type}/{Value}")]
    {

        return Ok(key);
    }
}
