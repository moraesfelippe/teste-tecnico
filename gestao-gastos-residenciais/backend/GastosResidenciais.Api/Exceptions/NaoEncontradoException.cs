namespace GastosResidenciais.Api.Exceptions;

/// <summary>
/// Lançada quando um recurso solicitado (ex.: uma pessoa por id) não é
/// encontrado. Traduzida pelo ExceptionHandlingMiddleware em HTTP 404.
/// </summary>
public class NaoEncontradoException : Exception
{
    public NaoEncontradoException(string mensagem) : base(mensagem)
    {
    }
}
