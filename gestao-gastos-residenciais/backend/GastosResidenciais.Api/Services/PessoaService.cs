using Microsoft.EntityFrameworkCore;
using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Exceptions;
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Services;

/// <summary>
/// Implementa as regras de negócio relacionadas ao cadastro de pessoas.
/// </summary>
public class PessoaService : IPessoaService
{
    private readonly AppDbContext _context;

    public PessoaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PessoaDto>> ListarAsync()
    {
        return await _context.Pessoas
            .OrderBy(p => p.Nome)
            .Select(p => new PessoaDto(p.Id, p.Nome, p.Idade))
            .ToListAsync();
    }

    public async Task<PessoaDto> CriarAsync(CriarPessoaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
            throw new RegraDeNegocioException("O nome é obrigatório.");

        if (dto.Idade < 0 || dto.Idade > 120)
            throw new RegraDeNegocioException("Informe uma idade válida (entre 0 e 120 anos).");

        var pessoa = new Pessoa
        {
            Nome = dto.Nome.Trim(),
            Idade = dto.Idade
        };

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return new PessoaDto(pessoa.Id, pessoa.Nome, pessoa.Idade);
    }

    public async Task DeletarAsync(int id)
    {
        // Carrega a pessoa junto com suas transações: dessa forma, o
        // Entity Framework Core aplica o delete em cascata configurado no
        // AppDbContext e remove tanto a pessoa quanto todas as suas
        // transações em uma única operação (SaveChangesAsync).
        var pessoa = await _context.Pessoas
            .Include(p => p.Transacoes)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pessoa is null)
            throw new NaoEncontradoException($"Pessoa com id {id} não foi encontrada.");

        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();
    }
}
