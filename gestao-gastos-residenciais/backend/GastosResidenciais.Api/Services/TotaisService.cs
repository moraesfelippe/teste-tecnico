using Microsoft.EntityFrameworkCore;
using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Services;

/// <summary>
/// Implementa o cálculo de totais de receitas, despesas e saldo por
/// pessoa, além do total geral do sistema.
/// </summary>
public class TotaisService : ITotaisService
{
    private readonly AppDbContext _context;

    public TotaisService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TotalGeralDto> ConsultarAsync()
    {
        var pessoas = await _context.Pessoas
            .Include(p => p.Transacoes)
            .OrderBy(p => p.Nome)
            .ToListAsync();

        var totaisPorPessoa = pessoas
            .Select(CalcularTotalDaPessoa)
            .ToList();

        var totalReceitas = totaisPorPessoa.Sum(t => t.TotalReceitas);
        var totalDespesas = totaisPorPessoa.Sum(t => t.TotalDespesas);

        return new TotalGeralDto(
            totaisPorPessoa,
            totalReceitas,
            totalDespesas,
            totalReceitas - totalDespesas);
    }

    /// <summary>Soma as receitas e despesas de uma pessoa e calcula o saldo dela.</summary>
    private static TotalPessoaDto CalcularTotalDaPessoa(Pessoa pessoa)
    {
        var totalReceitas = pessoa.Transacoes
            .Where(t => t.Tipo == TipoTransacao.Receita)
            .Sum(t => t.Valor);

        var totalDespesas = pessoa.Transacoes
            .Where(t => t.Tipo == TipoTransacao.Despesa)
            .Sum(t => t.Valor);

        return new TotalPessoaDto(
            pessoa.Id,
            pessoa.Nome,
            totalReceitas,
            totalDespesas,
            totalReceitas - totalDespesas);
    }
}
