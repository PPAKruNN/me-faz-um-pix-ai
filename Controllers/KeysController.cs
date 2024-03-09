
using FazUmPix.DTOs;
using FazUmPix.Services;
using Microsoft.AspNetCore.Mvc;

namespace FazUmPix.Controllers;

[ApiController]
[Route("[controller]")]
public class KeysController(KeysService keysService) : ControllerBase
{
    [HttpGet("/Keys/{Type}/{Value}")]
    public async Task<IActionResult> Read([FromRoute] ReadKeyInputDTO dto)
    {
        ReadKeyOutputDTO key = await keysService.ReadKey(dto);

        return Ok(key);
    }
}
