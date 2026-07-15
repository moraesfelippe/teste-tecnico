namespace GastosResidenciais.Api.Exceptions;

// vira HTTP 404 no middleware
public class NaoEncontradoException : Exception
{
    public NaoEncontradoException(string mensagem) : base(mensagem)
    {
    }
}
