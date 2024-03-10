
using FazUmPix.DTOs;
using FazUmPix.Models;
using FazUmPix.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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

        return CreatedAtAction(null, null, key);
    }

    [HttpGet("/Keys/{Type}/{Value}")]
    public async Task<IActionResult> Read([FromHeader] string Authorization, [FromRoute] ReadKeyInputDTO dto)
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


        ReadKeyOutputDTO key = await keysService.ReadKey(dto, token);

        return Ok(key);
    }
}
