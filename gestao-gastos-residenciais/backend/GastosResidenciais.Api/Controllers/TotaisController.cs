using Microsoft.AspNetCore.Mvc;
using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Services;

namespace GastosResidenciais.Api.Controllers;

/// <summary>Endpoint de consulta consolidada de totais por pessoa e geral.</summary>
[ApiController]
[Route("api/[controller]")]
public class TotaisController : ControllerBase
{
    private readonly ITotaisService _totaisService;

    public TotaisController(ITotaisService totaisService)
    {
        _totaisService = totaisService;
    }

    /// <summary>
    /// GET /api/totais — retorna o total de receitas, despesas e saldo de
    /// cada pessoa cadastrada, seguido do total geral (soma de todas as
    /// pessoas).
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<TotalGeralDto>> Consultar()
    {
        var totais = await _totaisService.ConsultarAsync();
        return Ok(totais);
    }
}
