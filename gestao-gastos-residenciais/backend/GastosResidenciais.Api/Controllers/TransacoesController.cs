using Microsoft.AspNetCore.Mvc;
using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Services;

namespace GastosResidenciais.Api.Controllers;

/// <summary>Endpoints para gerenciar o cadastro de transações financeiras.</summary>
[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoService _transacaoService;

    public TransacoesController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    /// <summary>GET /api/transacoes — lista todas as transações cadastradas.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransacaoDto>>> Listar()
    {
        var transacoes = await _transacaoService.ListarAsync();
        return Ok(transacoes);
    }

    /// <summary>
    /// POST /api/transacoes — cadastra uma nova transação. Pessoas menores
    /// de 18 anos só podem cadastrar despesas (ver regra em TransacaoService).
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TransacaoDto>> Criar([FromBody] CriarTransacaoDto dto)
    {
        var transacao = await _transacaoService.CriarAsync(dto);
        return CreatedAtAction(nameof(Listar), new { id = transacao.Id }, transacao);
    }
}
