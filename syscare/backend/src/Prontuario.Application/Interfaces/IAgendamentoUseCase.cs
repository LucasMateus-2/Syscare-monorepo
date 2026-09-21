using Prontuario.Application.DTOs;

namespace Prontuario.Application.Interfaces;

public interface IAgendamentoUseCase
{
    Task<AgendaDoDiaResponse> ObterAgendaDoDiaAsync(string? data, CancellationToken cancellationToken = default);
    Task<AgendamentoDto> CriarAsync(AgendamentoRequest request, CancellationToken cancellationToken = default);
    Task<AgendamentoDto> AtualizarStatusAsync(int id, AgendamentoStatusRequest request, CancellationToken cancellationToken = default);
    Task<AgendamentoDto> AtualizarAsync(int id, AgendamentoRequest request, CancellationToken cancellationToken = default);
    Task ExcluirAsync(int id, CancellationToken cancellationToken = default);
    Task<List<PacienteBuscaDto>> BuscarPacientesAsync(string? busca, CancellationToken cancellationToken = default);
    Task<List<AgendamentoDto>> ListarProximosPorPacienteAsync(int pacienteId, CancellationToken cancellationToken = default);
}
