using Microsoft.EntityFrameworkCore;
using Prontuario.Domain.Entities;

namespace Prontuario.Application.Interfaces;

/// <summary>
/// Abstração do DbContext usada pela camada Application, para que ela não
/// dependa diretamente do EF Core/Npgsql (implementados na Infrastructure).
/// </summary>
public interface IApplicationDbContext
{
    DbSet<Admin> Admins { get; }
    DbSet<Paciente> Pacientes { get; }
    DbSet<AvaliacaoSaude> AvaliacoesSaude { get; }
    DbSet<ExameFisico> ExamesFisicos { get; }
    DbSet<Ferida> Feridas { get; }
    DbSet<FeridaEvolucao> FeridaEvolucoes { get; }
    DbSet<DiagnosticoPaciente> DiagnosticosPaciente { get; }
    DbSet<DiagnosticoAvaliacao> DiagnosticoAvaliacoes { get; }
    DbSet<DiagnosticoAtividade> DiagnosticoAtividades { get; }
    DbSet<Prescricao> Prescricoes { get; }
    DbSet<Agendamento> Agendamentos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
