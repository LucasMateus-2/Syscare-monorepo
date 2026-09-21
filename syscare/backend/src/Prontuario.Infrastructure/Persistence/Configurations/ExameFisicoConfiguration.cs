using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prontuario.Domain.Entities;

namespace Prontuario.Infrastructure.Persistence.Configurations;

public class ExameFisicoConfiguration : IEntityTypeConfiguration<ExameFisico>
{
    public void Configure(EntityTypeBuilder<ExameFisico> builder)
    {
        builder.ToTable("exame_fisico");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.PacienteId).HasColumnName("paciente_id");

        builder.Property(e => e.Dados)
            .HasColumnName("dados")
            .HasColumnType("jsonb")
            .HasConversion(JsonValueConverters.ObjectMapConverter, JsonValueConverters.ObjectMapComparer)
            .IsRequired();

        builder.Property(e => e.CriadoEm).HasColumnName("criado_em");

        builder.HasIndex(e => e.PacienteId);
    }
}
