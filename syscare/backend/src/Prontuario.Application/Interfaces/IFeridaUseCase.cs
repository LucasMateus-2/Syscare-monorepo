using Prontuario.Application.DTOs;

namespace Prontuario.Application.Interfaces;

public interface IFeridaUseCase
{
    Task<FeridaDto> RegistrarAsync(int pacienteId, FeridaFormRequest request, IUploadedFile? foto, CancellationToken cancellationToken = default);
    Task<FeridaComEvolucoesResponse> ObterAsync(int pacienteId, int feridaId, CancellationToken cancellationToken = default);
    Task<FeridaEvolucaoDto> RegistrarEvolucaoAsync(int pacienteId, int feridaId, FeridaEvolucaoRequest request, IUploadedFile? foto, CancellationToken cancellationToken = default);
    Task EncerrarAsync(int pacienteId, int feridaId, CancellationToken cancellationToken = default);
}
