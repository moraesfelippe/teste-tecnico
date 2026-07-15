using Microsoft.EntityFrameworkCore;
using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Exceptions;
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Services;

public class TransacaoService : ITransacaoService
{
    private const int IdadeMinimaParaReceita = 18;

    private readonly AppDbContext _context;

    public TransacaoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TransacaoDto>> ListarAsync()
    {
        return await _context.Transacoes
            .OrderByDescending(t => t.Id)
            .Select(t => new TransacaoDto(t.Id, t.Descricao, t.Valor, t.Tipo, t.PessoaId))
            .ToListAsync();
    }

    public async Task<TransacaoDto> CriarAsync(CriarTransacaoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Descricao))
            throw new RegraDeNegocioException("A descrição é obrigatória.");

        if (dto.Valor <= 0)
            throw new RegraDeNegocioException("O valor deve ser maior que zero.");

        var pessoa = await _context.Pessoas.FindAsync(dto.PessoaId);
        if (pessoa is null)
        {
            throw new RegraDeNegocioException(
                $"Pessoa com id {dto.PessoaId} não foi encontrada. Cadastre a pessoa antes de lançar a transação.");
        }

        // menor de idade só pode cadastrar despesa
        if (pessoa.Idade < IdadeMinimaParaReceita && dto.Tipo == TipoTransacao.Receita)
        {
            throw new RegraDeNegocioException(
                $"{pessoa.Nome} é menor de idade ({pessoa.Idade} anos) e só pode cadastrar despesas, não receitas.");
        }

        var transacao = new Transacao
        {
            Descricao = dto.Descricao.Trim(),
            Valor = dto.Valor,
            Tipo = dto.Tipo,
            PessoaId = dto.PessoaId
        };

        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();

        return new TransacaoDto(transacao.Id, transacao.Descricao, transacao.Valor, transacao.Tipo, transacao.PessoaId);
    }
}