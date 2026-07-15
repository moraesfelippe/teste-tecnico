namespace GastosResidenciais.Api.DTOs;

/// <summary>Totais financeiros de uma única pessoa.</summary>
/// <param name="PessoaId">Identificador da pessoa.</param>
/// <param name="Nome">Nome da pessoa.</param>
/// <param name="TotalReceitas">Soma de todas as receitas da pessoa.</param>
/// <param name="TotalDespesas">Soma de todas as despesas da pessoa.</param>
/// <param name="Saldo">Receitas menos despesas.</param>
public record TotalPessoaDto(
    int PessoaId,
    string Nome,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Saldo);

/// <summary>
/// Consolidado geral: os totais de cada pessoa cadastrada, seguidos do
/// total geral somando todas as pessoas.
/// </summary>
/// <param name="Pessoas">Totais individuais de cada pessoa.</param>
/// <param name="TotalReceitas">Soma das receitas de todas as pessoas.</param>
/// <param name="TotalDespesas">Soma das despesas de todas as pessoas.</param>
/// <param name="Saldo">Saldo líquido geral (receitas - despesas).</param>
public record TotalGeralDto(
    List<TotalPessoaDto> Pessoas,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Saldo);
