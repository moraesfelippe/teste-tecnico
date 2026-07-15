using GastosResidenciais.Api.DTOs;

namespace GastosResidenciais.Api.Services;

public interface ITransacaoService
{
    Task<IEnumerable<TransacaoDto>> ListarAsync();
    Task<TransacaoDto> CriarAsync(CriarTransacaoDto dto);
}