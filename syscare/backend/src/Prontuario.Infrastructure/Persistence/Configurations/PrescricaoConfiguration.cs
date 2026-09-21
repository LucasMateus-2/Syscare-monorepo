using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prontuario.Domain.Entities;

namespace Prontuario.Infrastructure.Persistence.Configurations;

public class PrescricaoConfiguration : IEntityTypeConfiguration<Prescricao>
{
    public void Configure(EntityTypeBuilder<Prescricao> builder)
    {
        builder.ToTable("prescricoes");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("id");
        builder.Property(p => p.PacienteId).HasColumnName("paciente_id");
        builder.Property(p => p.Data).HasColumnName("data");
        builder.Property(p => p.AosCuidadosDe).HasColumnName("aos_cuidados_de");
        builder.Property(p => p.LimpezaSf09).HasColumnName("limpeza_sf09");
        builder.Property(p => p.Phmb).HasColumnName("phmb");
        builder.Property(p => p.CremeBarreira).HasColumnName("creme_barreira");
        builder.Property(p => p.Cobertura).HasColumnName("cobertura");

        builder.Property(p => p.CobrirCom)
            .HasColumnName("cobrir_com")
            .HasColumnType("jsonb")
            .HasConversion(JsonValueConverters.StringListConverter, JsonValueConverters.StringListComparer);

        builder.Property(p => p.FrequenciaTroca).HasColumnName("frequencia_troca");
        builder.Property(p => p.Observacoes).HasColumnName("observacoes");
        builder.Property(p => p.CriadoEm).HasColumnName("criado_em");

        builder.HasIndex(p => p.PacienteId);
    }
}
