namespace GastosResidenciais.Api.DTOs;

/// <summary>
/// Dados recebidos pela API para cadastrar uma nova pessoa.
/// Mantido separado da entidade Pessoa para não expor detalhes internos
/// do banco de dados (ex.: a lista de transações) na entrada da API.
/// </summary>
/// <param name="Nome">Nome completo da pessoa.</param>
/// <param name="Idade">Idade da pessoa, em anos completos.</param>
public record CriarPessoaDto(string Nome, int Idade);

/// <summary>Representação de uma pessoa retornada pela API.</summary>
public record PessoaDto(int Id, string Nome, int Idade);
