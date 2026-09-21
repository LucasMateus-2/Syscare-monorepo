using Prontuario.Application.Interfaces;

namespace Prontuario.Infrastructure.Services;

public class BcryptPasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12;

    public string Hash(string senha) => BCrypt.Net.BCrypt.HashPassword(senha, WorkFactor);

    public bool Verify(string senha, string hash) => BCrypt.Net.BCrypt.Verify(senha, hash);
}
