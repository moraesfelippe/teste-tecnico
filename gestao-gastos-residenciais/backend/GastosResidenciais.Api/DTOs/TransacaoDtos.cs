using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.DTOs;

/// <summary>
/// Dados recebidos pela API para cadastrar uma nova transação.
/// </summary>
/// <param name="Descricao">Descrição livre da transação.</param>
/// <param name="Valor">Valor monetário (deve ser maior que zero).</param>
/// <param name="Tipo">Receita ou despesa.</param>
/// <param name="PessoaId">Identificador da pessoa dona da transação. Precisa existir no cadastro de pessoas.</param>
public record CriarTransacaoDto(string Descricao, decimal Valor, TipoTransacao Tipo, int PessoaId);

/// <summary>Representação de uma transação retornada pela API.</summary>
public record TransacaoDto(int Id, string Descricao, decimal Valor, TipoTransacao Tipo, int PessoaId);
