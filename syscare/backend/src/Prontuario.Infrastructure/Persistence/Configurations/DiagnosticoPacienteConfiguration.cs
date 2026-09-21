using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prontuario.Domain.Entities;

namespace Prontuario.Infrastructure.Persistence.Configurations;

public class DiagnosticoPacienteConfiguration : IEntityTypeConfiguration<DiagnosticoPaciente>
{
    public void Configure(EntityTypeBuilder<DiagnosticoPaciente> builder)
    {
        builder.ToTable("diagnosticos_paciente");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("id");
        builder.Property(d => d.PacienteId).HasColumnName("paciente_id");
        builder.Property(d => d.DiagnosticoChave).HasColumnName("diagnostico_chave").IsRequired();
        builder.Property(d => d.Ativo).HasColumnName("ativo");
        builder.Property(d => d.CriadoEm).HasColumnName("criado_em");

        builder.HasIndex(d => new { d.PacienteId, d.DiagnosticoChave }).IsUnique();

        builder.HasMany(d => d.Avaliacoes)
            .WithOne(a => a.DiagnosticoPaciente)
            .HasForeignKey(a => a.DiagnosticoPacienteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Atividades)
            .WithOne(a => a.DiagnosticoPaciente)
            .HasForeignKey(a => a.DiagnosticoPacienteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
