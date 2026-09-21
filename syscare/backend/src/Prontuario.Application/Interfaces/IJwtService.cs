using Prontuario.Domain.Entities;

namespace Prontuario.Application.Interfaces;

public interface IJwtService
{
    /// <summary>Gera o token JWT assinado com os dados públicos do admin.</summary>
    string GerarToken(Admin admin);
}
