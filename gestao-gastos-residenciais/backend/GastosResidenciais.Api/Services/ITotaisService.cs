using GastosResidenciais.Api.DTOs;

namespace GastosResidenciais.Api.Services;

/// <summary>Contrato do serviço de consulta de totais.</summary>
public interface ITotaisService
{
    /// <summary>
    /// Calcula o total de receitas, despesas e saldo de cada pessoa
    /// cadastrada, além do total geral somando todas as pessoas.
    /// </summary>
    Task<TotalGeralDto> ConsultarAsync();
}
