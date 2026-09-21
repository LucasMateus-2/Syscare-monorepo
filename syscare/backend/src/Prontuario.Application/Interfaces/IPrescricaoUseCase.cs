using Prontuario.Application.DTOs;

namespace Prontuario.Application.Interfaces;

public interface IPrescricaoUseCase
{
    Task<PrescricaoDto> RegistrarAsync(int pacienteId, PrescricaoRequest request, CancellationToken cancellationToken = default);
    Task<PrescricaoComPacienteResponse> ObterAsync(int pacienteId, int prescricaoId, CancellationToken cancellationToken = default);
}
