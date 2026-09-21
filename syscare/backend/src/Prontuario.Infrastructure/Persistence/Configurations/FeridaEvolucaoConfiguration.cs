using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prontuario.Domain.Entities;

namespace Prontuario.Infrastructure.Persistence.Configurations;

public class FeridaEvolucaoConfiguration : IEntityTypeConfiguration<FeridaEvolucao>
{
    public void Configure(EntityTypeBuilder<FeridaEvolucao> builder)
    {
        builder.ToTable("ferida_evolucoes");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.FeridaId).HasColumnName("ferida_id");
        builder.Property(e => e.Data).HasColumnName("data").IsRequired();
        builder.Property(e => e.Comprimento).HasColumnName("comprimento");
        builder.Property(e => e.Largura).HasColumnName("largura");
        builder.Property(e => e.Profundidade).HasColumnName("profundidade");
        builder.Property(e => e.ExsudatoQuantidade).HasColumnName("exsudato_quantidade");
        builder.Property(e => e.Observacoes).HasColumnName("observacoes");
        builder.Property(e => e.Foto).HasColumnName("foto");
        builder.Property(e => e.CriadoEm).HasColumnName("criado_em");

        builder.HasIndex(e => e.FeridaId);
    }
}
