using Microsoft.EntityFrameworkCore;
using Prontuario.Application.Interfaces;
using Prontuario.Domain.Entities;

namespace Prontuario.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<AvaliacaoSaude> AvaliacoesSaude => Set<AvaliacaoSaude>();
    public DbSet<ExameFisico> ExamesFisicos => Set<ExameFisico>();
    public DbSet<Ferida> Feridas => Set<Ferida>();
    public DbSet<FeridaEvolucao> FeridaEvolucoes => Set<FeridaEvolucao>();
    public DbSet<DiagnosticoPaciente> DiagnosticosPaciente => Set<DiagnosticoPaciente>();
    public DbSet<DiagnosticoAvaliacao> DiagnosticoAvaliacoes => Set<DiagnosticoAvaliacao>();
    public DbSet<DiagnosticoAtividade> DiagnosticoAtividades => Set<DiagnosticoAtividade>();
    public DbSet<Prescricao> Prescricoes => Set<Prescricao>();
    public DbSet<Agendamento> Agendamentos => Set<Agendamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
