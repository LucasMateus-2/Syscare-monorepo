using Prontuario.Application.DTOs;

namespace Prontuario.Application.Interfaces;

public record LoginResultado(AdminDto Admin, string Token);

public interface IAuthUseCase
{
    Task<AuthStatusResponse> ObterStatusAsync(AdminDto? adminAutenticado, CancellationToken cancellationToken = default);
    Task<LoginResultado> ConfigurarAsync(ConfigurarRequest request, CancellationToken cancellationToken = default);
    Task<LoginResultado> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
