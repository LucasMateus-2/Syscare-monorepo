using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prontuario.Domain.Entities;

namespace Prontuario.Infrastructure.Persistence.Configurations;

public class AvaliacaoSaudeConfiguration : IEntityTypeConfiguration<AvaliacaoSaude>
{
    public void Configure(EntityTypeBuilder<AvaliacaoSaude> builder)
    {
        builder.ToTable("avaliacao_saude");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.PacienteId).HasColumnName("paciente_id");
        builder.Property(a => a.DoencaBase).HasColumnName("doenca_base");

        builder.Property(a => a.DoencaBaseQuais)
            .HasColumnName("doenca_base_quais")
            .HasColumnType("jsonb")
            .HasConversion(JsonValueConverters.StringListConverter, JsonValueConverters.StringListComparer);

        builder.Property(a => a.Medicacoes).HasColumnName("medicacoes");
        builder.Property(a => a.MedicacoesQuais).HasColumnName("medicacoes_quais");
        builder.Property(a => a.Alergias).HasColumnName("alergias");
        builder.Property(a => a.AlergiasQuais).HasColumnName("alergias_quais");
        builder.Property(a => a.Cirurgias).HasColumnName("cirurgias");
        builder.Property(a => a.CirurgiasQuais).HasColumnName("cirurgias_quais");
        builder.Property(a => a.Mobilidade).HasColumnName("mobilidade");
        builder.Property(a => a.MobilidadeObs).HasColumnName("mobilidade_obs");
        builder.Property(a => a.Higiene).HasColumnName("higiene");
        builder.Property(a => a.HigieneObs).HasColumnName("higiene_obs");
        builder.Property(a => a.CuidadoFerida).HasColumnName("cuidado_ferida");
        builder.Property(a => a.CuidadoFeridaQuem).HasColumnName("cuidado_ferida_quem");
        builder.Property(a => a.Alimentacao).HasColumnName("alimentacao");
        builder.Property(a => a.AtividadeFisica).HasColumnName("atividade_fisica");

        builder.Property(a => a.Habitos)
            .HasColumnName("habitos")
            .HasColumnType("jsonb")
            .HasConversion(JsonValueConverters.StringListConverter, JsonValueConverters.StringListComparer);

        builder.Property(a => a.Sono)
            .HasColumnName("sono")
            .HasColumnType("jsonb")
            .HasConversion(JsonValueConverters.StringListConverter, JsonValueConverters.StringListComparer);

        builder.Property(a => a.SonoMedicacao).HasColumnName("sono_medicacao");
        builder.Property(a => a.AtualizadoEm).HasColumnName("atualizado_em");

        builder.HasIndex(a => a.PacienteId).IsUnique();
    }
}
