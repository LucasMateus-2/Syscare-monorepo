using Microsoft.EntityFrameworkCore;
using Prontuario.Application.Common;
using Prontuario.Application.DTOs;
using Prontuario.Application.Interfaces;
using Prontuario.Domain.Common;
using Prontuario.Domain.Entities;

namespace Prontuario.Application.UseCases;

public class AgendamentoUseCase : IAgendamentoUseCase
{
    private readonly IApplicationDbContext _db;

    public AgendamentoUseCase(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<AgendaDoDiaResponse> ObterAgendaDoDiaAsync(string? data, CancellationToken cancellationToken = default)
    {
        var dia = Formatos.ParseDataIsoOuHoje(data, "Data");
        var inicioMes = new DateOnly(dia.Year, dia.Month, 1);
        var fimMes = inicioMes.AddMonths(1);

        var agsDia = await _db.Agendamentos
            .Include(a => a.Paciente)
            .Where(a => a.Data == dia)
            .OrderBy(a => a.Hora).ThenBy(a => a.Id)
            .ToListAsync(cancellationToken);

        var contagens = await _db.Agendamentos
            .Where(a => a.Data >= inicioMes && a.Data < fimMes && a.Status != "cancelado")
            .GroupBy(a => a.Data)
            .Select(g => new { Data = g.Key, Total = g.Count() })
            .ToListAsync(cancellationToken);

        return new AgendaDoDiaResponse
        {
            Data = dia.ToString("yyyy-MM-dd"),
            Agendamentos = agsDia.Select(a => a.ToDto()).ToList(),
            ContagemPorDia = contagens.ToDictionary(c => c.Data.ToString("yyyy-MM-dd"), c => c.Total),
        };
    }

    public async Task<AgendamentoDto> CriarAsync(AgendamentoRequest request, CancellationToken cancellationToken = default)
    {
        var data = Formatos.ParseDataIso(request.Data, "Data");
        var hora = Formatos.ParseHora(request.Hora);
        var duracao = ValidarDuracao(request.DuracaoMin);

        int? pacienteId = null;
        if (request.PacienteId is > 0)
        {
            var existe = await _db.Pacientes.AnyAsync(p => p.Id == request.PacienteId && p.Ativo, cancellationToken);
            if (!existe) throw AppException.NotFound("Paciente não encontrado.");
            pacienteId = request.PacienteId;
        }

        var agendamento = new Agendamento
        {
            PacienteId = pacienteId,
            NomePacienteAvulso = Formatos.TextoOuNulo(request.NomePacienteAvulso),
            TelefoneAvulso = Formatos.TextoOuNulo(request.TelefoneAvulso),
            Data = data,
            Hora = hora,
            DuracaoMin = duracao,
            Procedimento = Formatos.TextoOuNulo(request.Procedimento),
            Observacoes = Formatos.TextoOuNulo(request.Observacoes),
        };

        _db.Agendamentos.Add(agendamento);
        await _db.SaveChangesAsync(cancellationToken);
        return agendamento.ToDto();
    }

    public async Task<AgendamentoDto> AtualizarStatusAsync(int id, AgendamentoStatusRequest request, CancellationToken cancellationToken = default)
    {
        var agendamento = await ExigirAgendamentoAsync(id, cancellationToken);

        if (!Agendamento.StatusPermitidos.Contains(request.Status))
            throw AppException.BadRequest("Status inválido.");

        agendamento.Status = request.Status;
        await _db.SaveChangesAsync(cancellationToken);
        return agendamento.ToDto();
    }

    public async Task<AgendamentoDto> AtualizarAsync(int id, AgendamentoRequest request, CancellationToken cancellationToken = default)
    {
        var agendamento = await ExigirAgendamentoAsync(id, cancellationToken);

        agendamento.Data = Formatos.ParseDataIso(request.Data, "Data");
        agendamento.Hora = Formatos.ParseHora(request.Hora);
        agendamento.DuracaoMin = ValidarDuracao(request.DuracaoMin);
        agendamento.Procedimento = Formatos.TextoOuNulo(request.Procedimento);
        agendamento.Observacoes = Formatos.TextoOuNulo(request.Observacoes);

        await _db.SaveChangesAsync(cancellationToken);
        return agendamento.ToDto();
    }

    public async Task ExcluirAsync(int id, CancellationToken cancellationToken = default)
    {
        var agendamento = await ExigirAgendamentoAsync(id, cancellationToken);
        _db.Agendamentos.Remove(agendamento);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<PacienteBuscaDto>> BuscarPacientesAsync(string? busca, CancellationToken cancellationToken = default)
    {
        var termo = (busca ?? string.Empty).Trim();
        if (termo.Length == 0) return new List<PacienteBuscaDto>();

        return await _db.Pacientes
            .Where(p => p.Ativo && EF.Functions.Like(p.Nome, $"%{termo}%"))
            .OrderBy(p => p.Nome)
            .Take(8)
            .Select(p => new PacienteBuscaDto { Id = p.Id, Nome = p.Nome, Telefone = p.Telefone, Foto = p.Foto })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<AgendamentoDto>> ListarProximosPorPacienteAsync(int pacienteId, CancellationToken cancellationToken = default)
    {
        if (pacienteId <= 0) throw AppException.BadRequest("Paciente inválido.");
        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);

        var agendamentos = await _db.Agendamentos
            .Where(a => a.PacienteId == pacienteId && a.Data >= hoje && a.Status != "cancelado")
            .OrderBy(a => a.Data).ThenBy(a => a.Hora).ThenBy(a => a.Id)
            .ToListAsync(cancellationToken);

        return agendamentos.Select(a => a.ToDto()).ToList();
    }

    private static int ValidarDuracao(int? valor)
    {
        if (valor is null) return 30;
        if (valor < 5) throw AppException.BadRequest("Duração deve ser um número inteiro maior ou igual a 5.");
        return valor.Value;
    }

    private async Task<Agendamento> ExigirAgendamentoAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) throw AppException.BadRequest("Agendamento inválido.");
        var agendamento = await _db.Agendamentos.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        return agendamento ?? throw AppException.NotFound("Agendamento não encontrado.");
    }
}
