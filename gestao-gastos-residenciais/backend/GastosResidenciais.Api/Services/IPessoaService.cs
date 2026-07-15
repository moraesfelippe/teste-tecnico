using GastosResidenciais.Api.DTOs;

namespace GastosResidenciais.Api.Services;

public interface IPessoaService
{
    Task<IEnumerable<PessoaDto>> ListarAsync();
    Task<PessoaDto> CriarAsync(CriarPessoaDto dto);
    Task DeletarAsync(int id);
}