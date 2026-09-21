using Microsoft.EntityFrameworkCore;
using Prontuario.Application.Common;
using Prontuario.Application.DTOs;
using Prontuario.Application.Interfaces;
using Prontuario.Domain.Common;
using Prontuario.Domain.Entities;

namespace Prontuario.Application.UseCases;

public class PrescricaoUseCase : IPrescricaoUseCase
{
    private readonly IApplicationDbContext _db;

    public PrescricaoUseCase(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PrescricaoDto> RegistrarAsync(int pacienteId, PrescricaoRequest request, CancellationToken cancellationToken = default)
    {
        if (pacienteId <= 0) throw AppException.BadRequest("Paciente inválido.");
        var existe = await _db.Pacientes.AnyAsync(p => p.Id == pacienteId, cancellationToken);
        if (!existe) throw AppException.NotFound("Paciente não encontrado.");

        var prescricao = new Prescricao
        {
            PacienteId = pacienteId,
            Data = Formatos.ParseDataIsoOuHoje(request.Data),
            AosCuidadosDe = Formatos.TextoOuNulo(request.AosCuidadosDe),
            LimpezaSf09 = request.LimpezaSf09,
            Phmb = request.Phmb,
            CremeBarreira = request.CremeBarreira,
            Cobertura = Formatos.TextoOuNulo(request.Cobertura),
            CobrirCom = Formatos.Lista(request.CobrirCom),
            FrequenciaTroca = Formatos.TextoOuNulo(request.FrequenciaTroca),
            Observacoes = Formatos.TextoOuNulo(request.Observacoes),
        };

        _db.Prescricoes.Add(prescricao);
        await _db.SaveChangesAsync(cancellationToken);
        return prescricao.ToDto();
    }

    public async Task<PrescricaoComPacienteResponse> ObterAsync(int pacienteId, int prescricaoId, CancellationToken cancellationToken = default)
    {
        if (pacienteId <= 0) throw AppException.BadRequest("Paciente inválido.");
        if (prescricaoId <= 0) throw AppException.BadRequest("Prescrição inválida.");

        var paciente = await _db.Pacientes.FirstOrDefaultAsync(p => p.Id == pacienteId, cancellationToken)
            ?? throw AppException.NotFound("Paciente não encontrado.");

        var prescricao = await _db.Prescricoes
            .FirstOrDefaultAsync(p => p.Id == prescricaoId && p.PacienteId == pacienteId, cancellationToken)
            ?? throw AppException.NotFound("Prescrição não encontrada.");

        return new PrescricaoComPacienteResponse
        {
            Paciente = paciente.ToDto(),
            Prescricao = prescricao.ToDto(),
        };
    }
}
