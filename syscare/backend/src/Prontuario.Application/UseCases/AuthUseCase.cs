using Microsoft.EntityFrameworkCore;
using Prontuario.Application.DTOs;
using Prontuario.Application.Interfaces;
using Prontuario.Domain.Common;
using Prontuario.Domain.Entities;

namespace Prontuario.Application.UseCases;

public class AuthUseCase : IAuthUseCase
{
    private readonly IApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public AuthUseCase(IApplicationDbContext db, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<AuthStatusResponse> ObterStatusAsync(AdminDto? adminAutenticado, CancellationToken cancellationToken = default)
    {
        var existeAdmin = await _db.Admins.AnyAsync(cancellationToken);
        return new AuthStatusResponse
        {
            HasAdmin = existeAdmin,
            LoggedIn = adminAutenticado is not null,
            Admin = adminAutenticado,
        };
    }

    public async Task<LoginResultado> ConfigurarAsync(ConfigurarRequest request, CancellationToken cancellationToken = default)
    {
        if (await _db.Admins.AnyAsync(cancellationToken))
            throw AppException.BadRequest("Já existe um administrador configurado.");

        var nome = string.IsNullOrWhiteSpace(request.Nome) ? "Administrador" : request.Nome.Trim();

        var admin = new Admin
        {
            Usuario = request.Usuario.Trim(),
            SenhaHash = _passwordHasher.Hash(request.Senha),
            Nome = nome,
        };

        _db.Admins.Add(admin);
        await _db.SaveChangesAsync(cancellationToken);

        var dto = ParaDto(admin);
        return new LoginResultado(dto, _jwtService.GerarToken(admin));
    }

    public async Task<LoginResultado> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var usuario = request.Usuario.Trim();
        var admin = await _db.Admins.FirstOrDefaultAsync(a => a.Usuario == usuario, cancellationToken);

        if (admin is null || !_passwordHasher.Verify(request.Senha, admin.SenhaHash))
            throw AppException.Unauthorized("Usuário ou senha incorretos.");

        var dto = ParaDto(admin);
        return new LoginResultado(dto, _jwtService.GerarToken(admin));
    }

    private static AdminDto ParaDto(Admin admin) => new()
    {
        Id = admin.Id,
        Usuario = admin.Usuario,
        Nome = admin.Nome,
    };
}
