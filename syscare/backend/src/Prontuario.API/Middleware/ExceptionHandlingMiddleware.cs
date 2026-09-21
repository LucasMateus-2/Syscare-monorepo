using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Prontuario.Domain.Common;

namespace Prontuario.API.Middleware;

public class ErroResponse
{
    [JsonPropertyName("erro")]
    public string Erro { get; set; } = null!;
}

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = null,
    };

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
        catch (AppException ex)
        {
            await EscreverAsync(context, ex.StatusCode, ex.Message);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg)
        {
            var (status, mensagem) = TraduzirErroPostgres(pg);
            _logger.LogWarning(ex, "Erro de banco de dados tratado como {Status}", status);
            await EscreverAsync(context, status, mensagem);
        }
        catch (BadHttpRequestException ex)
        {
            await EscreverAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado ao processar {Path}", context.Request.Path);
            await EscreverAsync(context, StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
        }
    }

    private static (int Status, string Mensagem) TraduzirErroPostgres(PostgresException pg) => pg.SqlState switch
    {
        "23505" => (StatusCodes.Status409Conflict, "Já existe um registro com esses dados."),
        "23503" => (StatusCodes.Status409Conflict, "Não é possível concluir a operação: registro relacionado inexistente."),
        "22P02" => (StatusCodes.Status400BadRequest, "Dado inválido enviado."),
        _ => (StatusCodes.Status500InternalServerError, "Erro interno do servidor."),
    };

    private static Task EscreverAsync(HttpContext context, int statusCode, string mensagem)
    {
        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        var corpo = JsonSerializer.Serialize(new ErroResponse { Erro = mensagem }, JsonOptions);
        return context.Response.WriteAsync(corpo);
    }
}
