using Prontuario.Application.DTOs;

namespace Prontuario.Application.Interfaces;

public interface IDiagnosticoUseCase
{
    Task ToggleAsync(int pacienteId, ToggleDiagnosticoRequest request, CancellationToken cancellationToken = default);
    Task<DiagnosticoDetalheResponse> ObterAsync(int pacienteId, string chave, CancellationToken cancellationToken = default);
    Task<DiagnosticoAvaliacaoDto> RegistrarAvaliacaoAsync(int pacienteId, string chave, DiagnosticoAvaliacaoRequest request, CancellationToken cancellationToken = default);
    Task<DiagnosticoAtividadeDto> RegistrarAtividadeAsync(int pacienteId, string chave, DiagnosticoAtividadeRequest request, CancellationToken cancellationToken = default);
}
