using Microsoft.EntityFrameworkCore;
using Prontuario.Application.Common;
using Prontuario.Application.DTOs;
using Prontuario.Application.Interfaces;
using Prontuario.Domain.Common;
using Prontuario.Domain.Entities;

namespace Prontuario.Application.UseCases;

public class DiagnosticoUseCase : IDiagnosticoUseCase
{
    private readonly IApplicationDbContext _db;

    public DiagnosticoUseCase(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task ToggleAsync(int pacienteId, ToggleDiagnosticoRequest request, CancellationToken cancellationToken = default)
    {
        await ExigirPacienteAsync(pacienteId, cancellationToken);
        var chave = ValidarChave(request.Chave);

        var registro = await _db.DiagnosticosPaciente
            .FirstOrDefaultAsync(d => d.PacienteId == pacienteId && d.DiagnosticoChave == chave, cancellationToken);

        if (registro is null)
        {
            _db.DiagnosticosPaciente.Add(new DiagnosticoPaciente
            {
                PacienteId = pacienteId,
                DiagnosticoChave = chave,
                Ativo = request.Ligar,
            });
        }
        else
        {
            registro.Ativo = request.Ligar;
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<DiagnosticoDetalheResponse> ObterAsync(int pacienteId, string chave, CancellationToken cancellationToken = default)
    {
        var dp = await ObterDiagnosticoPacienteAsync(pacienteId, chave, exigirAtivo: true, cancellationToken);

        var avaliacoes = await _db.DiagnosticoAvaliacoes
            .Where(a => a.DiagnosticoPacienteId == dp.Id)
            .OrderBy(a => a.Data).ThenBy(a => a.Id)
            .ToListAsync(cancellationToken);

        var atividades = await _db.DiagnosticoAtividades
            .Where(a => a.DiagnosticoPacienteId == dp.Id)
            .OrderBy(a => a.Data).ThenBy(a => a.Id)
            .ToListAsync(cancellationToken);

        var atividadesMap = new Dictionary<string, List<string>>();
        foreach (var atividade in atividades)
        {
            if (!atividadesMap.TryGetValue(atividade.Atividade, out var datas))
            {
                datas = new List<string>();
                atividadesMap[atividade.Atividade] = datas;
            }
            datas.Add(atividade.Data.ToString("yyyy-MM-dd"));
        }

        return new DiagnosticoDetalheResponse
        {
            DiagnosticoPaciente = dp.ToDto(),
            Avaliacoes = avaliacoes.Select(a => a.ToDto()).ToList(),
            AtividadesMap = atividadesMap,
        };
    }

    public async Task<DiagnosticoAvaliacaoDto> RegistrarAvaliacaoAsync(int pacienteId, string chave, DiagnosticoAvaliacaoRequest request, CancellationToken cancellationToken = default)
    {
        var dp = await ObterDiagnosticoPacienteAsync(pacienteId, chave, exigirAtivo: true, cancellationToken);

        var avaliacao = new DiagnosticoAvaliacao
        {
            DiagnosticoPacienteId = dp.Id,
            Data = Formatos.ParseDataIsoOuHoje(request.Data),
            Valores = request.Valores ?? new Dictionary<string, object?>(),
        };

        _db.DiagnosticoAvaliacoes.Add(avaliacao);
        await _db.SaveChangesAsync(cancellationToken);
        return avaliacao.ToDto();
    }

    public async Task<DiagnosticoAtividadeDto> RegistrarAtividadeAsync(int pacienteId, string chave, DiagnosticoAtividadeRequest request, CancellationToken cancellationToken = default)
    {
        var dp = await ObterDiagnosticoPacienteAsync(pacienteId, chave, exigirAtivo: true, cancellationToken);

        var atividade = new DiagnosticoAtividade
        {
            DiagnosticoPacienteId = dp.Id,
            Atividade = request.Atividade.Trim(),
            Data = Formatos.ParseDataIsoOuHoje(request.Data),
        };

        _db.DiagnosticoAtividades.Add(atividade);
        await _db.SaveChangesAsync(cancellationToken);
        return atividade.ToDto();
    }

    private static string ValidarChave(string? chave)
    {
        if (!DiagnosticosCatalog.EhValida(chave)) throw AppException.BadRequest("Diagnóstico inválido.");
        return chave!;
    }

    private async Task ExigirPacienteAsync(int pacienteId, CancellationToken cancellationToken)
    {
        if (pacienteId <= 0) throw AppException.BadRequest("Paciente inválido.");
        var existe = await _db.Pacientes.AnyAsync(p => p.Id == pacienteId, cancellationToken);
        if (!existe) throw AppException.NotFound("Paciente não encontrado.");
    }

    private async Task<DiagnosticoPaciente> ObterDiagnosticoPacienteAsync(int pacienteId, string? chave, bool exigirAtivo, CancellationToken cancellationToken)
    {
        await ExigirPacienteAsync(pacienteId, cancellationToken);
        var chaveValida = ValidarChave(chave);

        var query = _db.DiagnosticosPaciente.Where(d => d.PacienteId == pacienteId && d.DiagnosticoChave == chaveValida);
        if (exigirAtivo) query = query.Where(d => d.Ativo);

        var registro = await query.FirstOrDefaultAsync(cancellationToken);
        return registro ?? throw AppException.NotFound("Diagnóstico não ativado para este paciente.");
    }
}
