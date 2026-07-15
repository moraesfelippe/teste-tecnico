namespace GastosResidenciais.Api.Exceptions;

/// <summary>
/// Lançada quando uma requisição viola uma regra de negócio do domínio
/// (ex.: menor de idade tentando cadastrar uma receita, ou dados
/// obrigatórios ausentes). Traduzida pelo ExceptionHandlingMiddleware em
/// HTTP 400.
/// </summary>
public class RegraDeNegocioException : Exception
{
    public RegraDeNegocioException(string mensagem) : base(mensagem)
    {
    }
}
