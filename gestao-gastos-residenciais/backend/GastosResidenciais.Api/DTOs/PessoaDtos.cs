namespace GastosResidenciais.Api.DTOs;

public record CriarPessoaDto(string Nome, int Idade);
public record PessoaDto(int Id, string Nome, int Idade);