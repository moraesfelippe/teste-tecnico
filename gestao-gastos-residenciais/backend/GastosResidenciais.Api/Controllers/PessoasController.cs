using Microsoft.AspNetCore.Mvc;
using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Services;

namespace GastosResidenciais.Api.Controllers;

/// <summary>Endpoints para gerenciar o cadastro de pessoas.</summary>
[ApiController]
[Route("api/[controller]")]
public class PessoasController : ControllerBase
{
    private readonly IPessoaService _pessoaService;

    public PessoasController(IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    /// <summary>GET /api/pessoas — lista todas as pessoas cadastradas.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PessoaDto>>> Listar()
    {
        var pessoas = await _pessoaService.ListarAsync();
        return Ok(pessoas);
    }

    /// <summary>POST /api/pessoas — cadastra uma nova pessoa.</summary>
    [HttpPost]
    public async Task<ActionResult<PessoaDto>> Criar([FromBody] CriarPessoaDto dto)
    {
        var pessoa = await _pessoaService.CriarAsync(dto);
        return CreatedAtAction(nameof(Listar), new { id = pessoa.Id }, pessoa);
    }

    /// <summary>
    /// DELETE /api/pessoas/{id} — remove uma pessoa. Todas as transações
    /// associadas a ela são removidas automaticamente (delete em cascata).
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deletar(int id)
    {
        await _pessoaService.DeletarAsync(id);
        return NoContent();
    }
}
