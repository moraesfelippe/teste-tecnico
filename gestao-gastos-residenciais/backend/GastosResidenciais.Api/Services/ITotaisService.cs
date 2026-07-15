using GastosResidenciais.Api.DTOs;

namespace GastosResidenciais.Api.Services;

public interface ITotaisService
{
    Task<TotalGeralDto> ConsultarAsync();
}