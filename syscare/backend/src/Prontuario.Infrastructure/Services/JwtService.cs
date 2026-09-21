using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Prontuario.Application.Interfaces;
using Prontuario.Domain.Entities;

namespace Prontuario.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly JwtOptions _options;

    public JwtService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GerarToken(Admin admin)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
            new Claim("usuario", admin.Usuario),
            new Claim("nome", admin.Nome),
        };

        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_options.ExpiracaoHoras),
            signingCredentials: credenciais);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
