namespace GastosResidenciais.Api.DTOs;

public record TotalPessoaDto(
    int PessoaId,
    string Nome,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Saldo);

public record TotalGeralDto(
    List<TotalPessoaDto> Pessoas,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Saldo);