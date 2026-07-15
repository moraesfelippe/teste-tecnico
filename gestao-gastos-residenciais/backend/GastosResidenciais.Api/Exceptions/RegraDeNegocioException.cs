namespace GastosResidenciais.Api.Exceptions;

// vira HTTP 400 no middleware
public class RegraDeNegocioException : Exception
{
    public RegraDeNegocioException(string mensagem) : base(mensagem)
    {
    }
}
