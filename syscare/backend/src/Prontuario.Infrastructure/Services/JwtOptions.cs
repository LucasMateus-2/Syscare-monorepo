namespace Prontuario.Infrastructure.Services;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Secret { get; set; } = null!;
    public string Issuer { get; set; } = "Prontuario.API";
    public int ExpiracaoHoras { get; set; } = 12;
}
