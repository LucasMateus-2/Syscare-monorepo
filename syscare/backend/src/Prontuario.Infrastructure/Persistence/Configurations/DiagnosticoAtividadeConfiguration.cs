using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prontuario.Domain.Entities;

namespace Prontuario.Infrastructure.Persistence.Configurations;

public class DiagnosticoAtividadeConfiguration : IEntityTypeConfiguration<DiagnosticoAtividade>
{
    public void Configure(EntityTypeBuilder<DiagnosticoAtividade> builder)
    {
        builder.ToTable("diagnostico_atividades");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.DiagnosticoPacienteId).HasColumnName("diagnostico_paciente_id");
        builder.Property(a => a.Atividade).HasColumnName("atividade").IsRequired();
        builder.Property(a => a.Data).HasColumnName("data").IsRequired();
        builder.Property(a => a.CriadoEm).HasColumnName("criado_em");

        builder.HasIndex(a => a.DiagnosticoPacienteId);
    }
}
