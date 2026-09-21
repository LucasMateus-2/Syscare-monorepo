using Prontuario.Application.DTOs;

namespace Prontuario.Application.Interfaces;

public interface IPacienteUseCase
{
    Task<List<PacienteDto>> ListarAsync(string? busca, CancellationToken cancellationToken = default);
    Task<PacienteDto> CriarAsync(PacienteFormRequest request, IUploadedFile? foto, CancellationToken cancellationToken = default);
    Task<FichaPacienteResponse> ObterFichaAsync(int id, CancellationToken cancellationToken = default);
    Task<PacienteDto> AtualizarAsync(int id, PacienteFormRequest request, CancellationToken cancellationToken = default);
    Task ExcluirAsync(int id, CancellationToken cancellationToken = default);
    Task<PacienteDto> AtualizarFotoAsync(int id, IUploadedFile foto, CancellationToken cancellationToken = default);

    Task<AvaliacaoSaudeDto> SalvarAvaliacaoSaudeAsync(int id, AvaliacaoSaudeRequest request, CancellationToken cancellationToken = default);

    Task<ExameFisicoDto> RegistrarExameFisicoAsync(int id, ExameFisicoRequest request, CancellationToken cancellationToken = default);
    Task<List<ExameFisicoDto>> ObterHistoricoExameFisicoAsync(int id, CancellationToken cancellationToken = default);
}
