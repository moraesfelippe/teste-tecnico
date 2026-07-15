using GastosResidenciais.Api.DTOs;

namespace GastosResidenciais.Api.Services;

/// <summary>Contrato do serviço de gerenciamento de transações.</summary>
public interface ITransacaoService
{
    /// <summary>Lista todas as transações cadastradas, da mais recente para a mais antiga.</summary>
    Task<IEnumerable<TransacaoDto>> ListarAsync();

    /// <summary>
    /// Cadastra uma nova transação. Lança
    /// <see cref="Exceptions.RegraDeNegocioException"/> se a pessoa informada
    /// não existir ou se a regra de menor de idade for violada.
    /// </summary>
    Task<TransacaoDto> CriarAsync(CriarTransacaoDto dto);
}
