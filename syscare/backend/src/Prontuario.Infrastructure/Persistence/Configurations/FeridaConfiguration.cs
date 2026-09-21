using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prontuario.Domain.Entities;

namespace Prontuario.Infrastructure.Persistence.Configurations;

public class FeridaConfiguration : IEntityTypeConfiguration<Ferida>
{
    public void Configure(EntityTypeBuilder<Ferida> builder)
    {
        builder.ToTable("feridas");
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id).HasColumnName("id");
        builder.Property(f => f.PacienteId).HasColumnName("paciente_id");
        builder.Property(f => f.NumeroLesao).HasColumnName("numero_lesao");
        builder.Property(f => f.Etiologia).HasColumnName("etiologia");
        builder.Property(f => f.TempoFerida).HasColumnName("tempo_ferida");
        builder.Property(f => f.Localizacao).HasColumnName("localizacao");
        builder.Property(f => f.Comprimento).HasColumnName("comprimento");
        builder.Property(f => f.Largura).HasColumnName("largura");
        builder.Property(f => f.Profundidade).HasColumnName("profundidade");
        builder.Property(f => f.Descolamento).HasColumnName("descolamento");

        builder.Property(f => f.TimeTecido).HasColumnName("time_tecido").HasColumnType("jsonb")
            .HasConversion(JsonValueConverters.StringListConverter, JsonValueConverters.StringListComparer);
        builder.Property(f => f.TimeInfeccao).HasColumnName("time_infeccao").HasColumnType("jsonb")
            .HasConversion(JsonValueConverters.StringListConverter, JsonValueConverters.StringListComparer);

        builder.Property(f => f.ExsudatoTipo).HasColumnName("exsudato_tipo");
        builder.Property(f => f.ExsudatoQuantidade).HasColumnName("exsudato_quantidade");

        builder.Property(f => f.Bordas).HasColumnName("bordas").HasColumnType("jsonb")
            .HasConversion(JsonValueConverters.StringListConverter, JsonValueConverters.StringListComparer);
        builder.Property(f => f.Perilesional).HasColumnName("perilesional").HasColumnType("jsonb")
            .HasConversion(JsonValueConverters.StringListConverter, JsonValueConverters.StringListComparer);

        builder.Property(f => f.Biofilme).HasColumnName("biofilme");

        builder.Property(f => f.BiofilmeSinais).HasColumnName("biofilme_sinais").HasColumnType("jsonb")
            .HasConversion(JsonValueConverters.StringListConverter, JsonValueConverters.StringListComparer);

        builder.Property(f => f.ItbDados).HasColumnName("itb_dados").HasColumnType("jsonb")
            .HasConversion(JsonValueConverters.ObjectMapConverter, JsonValueConverters.ObjectMapComparer);

        builder.Property(f => f.Sensibilidade).HasColumnName("sensibilidade");
        builder.Property(f => f.SensibilidadeObs).HasColumnName("sensibilidade_obs");
        builder.Property(f => f.Foto).HasColumnName("foto");
        builder.Property(f => f.CriadoEm).HasColumnName("criado_em");
        builder.Property(f => f.Ativa).HasColumnName("ativa");

        builder.HasIndex(f => f.PacienteId);

        builder.HasMany(f => f.Evolucoes)
            .WithOne(e => e.Ferida)
            .HasForeignKey(e => e.FeridaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
