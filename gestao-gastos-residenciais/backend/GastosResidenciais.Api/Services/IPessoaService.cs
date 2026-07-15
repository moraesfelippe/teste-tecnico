using GastosResidenciais.Api.DTOs;

namespace GastosResidenciais.Api.Services;

/// <summary>Contrato do serviço de gerenciamento de pessoas.</summary>
public interface IPessoaService
{
    /// <summary>Lista todas as pessoas cadastradas, ordenadas por nome.</summary>
    Task<IEnumerable<PessoaDto>> ListarAsync();

    /// <summary>Cadastra uma nova pessoa.</summary>
    Task<PessoaDto> CriarAsync(CriarPessoaDto dto);

    /// <summary>
    /// Remove uma pessoa e, em cascata, todas as suas transações.
    /// Lança <see cref="Exceptions.NaoEncontradoException"/> se o id não existir.
    /// </summary>
    Task DeletarAsync(int id);
}
