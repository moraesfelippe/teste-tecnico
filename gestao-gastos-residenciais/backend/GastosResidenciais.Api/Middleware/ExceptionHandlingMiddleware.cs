using System.Net;
using System.Text.Json;
using GastosResidenciais.Api.Exceptions;

namespace GastosResidenciais.Api.Middleware;

/// <summary>
/// Middleware central de tratamento de erros da API. Converte exceções de
/// domínio (regra de negócio violada, recurso não encontrado) em respostas
/// HTTP com status e corpo JSON apropriados, evitando repetir blocos
/// try/catch em cada controller.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NaoEncontradoException ex)
        {
            await EscreverRespostaDeErro(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (RegraDeNegocioException ex)
        {
            await EscreverRespostaDeErro(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado ao processar a requisição.");
            await EscreverRespostaDeErro(
                context,
                HttpStatusCode.InternalServerError,
                "Ocorreu um erro inesperado ao processar a requisição.");
        }
    }

    private static async Task EscreverRespostaDeErro(HttpContext context, HttpStatusCode status, string mensagem)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        var payload = JsonSerializer.Serialize(new { erro = mensagem });
        await context.Response.WriteAsync(payload);
    }
}
