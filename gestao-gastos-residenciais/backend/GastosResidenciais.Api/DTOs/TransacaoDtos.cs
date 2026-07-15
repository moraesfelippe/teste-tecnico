using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.DTOs;

public record CriarTransacaoDto(string Descricao, decimal Valor, TipoTransacao Tipo, int PessoaId);
public record TransacaoDto(int Id, string Descricao, decimal Valor, TipoTransacao Tipo, int PessoaId);