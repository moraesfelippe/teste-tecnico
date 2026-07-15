using Microsoft.AspNetCore.Mvc;
using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Services;

namespace GastosResidenciais.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TotaisController : ControllerBase
{
    private readonly ITotaisService _totaisService;

    public TotaisController(ITotaisService totaisService)
    {
        _totaisService = totaisService;
    }

    [HttpGet]
    public async Task<ActionResult<TotalGeralDto>> Consultar()
    {
        var totais = await _totaisService.ConsultarAsync();
        return Ok(totais);
    }
}