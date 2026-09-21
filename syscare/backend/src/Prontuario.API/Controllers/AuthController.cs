using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prontuario.Application.DTOs;
using Prontuario.Application.Interfaces;

namespace Prontuario.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private const string CookieNome = "token";

    private readonly IAuthUseCase _authUseCase;
    private readonly IConfiguration _configuration;

    public AuthController(IAuthUseCase authUseCase, IConfiguration configuration)
    {
        _authUseCase = authUseCase;
        _configuration = configuration;
    }

    [HttpGet("status")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthStatusResponse>> Status(CancellationToken cancellationToken)
    {
        var adminAutenticado = ObterAdminAutenticado();
        var resultado = await _authUseCase.ObterStatusAsync(adminAutenticado, cancellationToken);
        return Ok(resultado);
    }

    [HttpPost("configurar")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Configurar([FromBody] ConfigurarRequest request, CancellationToken cancellationToken)
    {
        var resultado = await _authUseCase.ConfigurarAsync(request, cancellationToken);
        DefinirCookie(resultado.Token);
        return Ok(new AuthResponse { Admin = resultado.Admin });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var resultado = await _authUseCase.LoginAsync(request, cancellationToken);
        DefinirCookie(resultado.Token);
        return Ok(new AuthResponse { Admin = resultado.Admin });
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public ActionResult<OkResponse> Logout()
    {
        Response.Cookies.Delete(CookieNome, new CookieOptions { Path = "/" });
        return Ok(new OkResponse());
    }

    private AdminDto? ObterAdminAutenticado()
    {
        if (User.Identity?.IsAuthenticated != true) return null;

        var id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var usuario = User.FindFirst("usuario")?.Value;
        var nome = User.FindFirst("nome")?.Value;

        if (id is null || usuario is null || nome is null) return null;

        return new AdminDto { Id = int.Parse(id), Usuario = usuario, Nome = nome };
    }

    private void DefinirCookie(string token)
    {
        var horas = _configuration.GetValue<int?>("Jwt:ExpiracaoHoras") ?? 12;
        var usarHttps = _configuration.GetValue<bool?>("Cookies:Secure") ?? !HttpContext.Request.Host.Host.Contains("localhost");

        Response.Cookies.Append(CookieNome, token, new CookieOptions
        {
            HttpOnly = true,
            Secure = usarHttps,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddHours(horas),
        });
    }
}
