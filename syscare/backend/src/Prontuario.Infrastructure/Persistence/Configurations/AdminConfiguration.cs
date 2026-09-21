using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prontuario.Domain.Entities;

namespace Prontuario.Infrastructure.Persistence.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("admin");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.Usuario).HasColumnName("usuario").IsRequired();
        builder.Property(a => a.SenhaHash).HasColumnName("senha_hash").IsRequired();
        builder.Property(a => a.Nome).HasColumnName("nome").IsRequired();
        builder.Property(a => a.CriadoEm).HasColumnName("criado_em");

        builder.HasIndex(a => a.Usuario).IsUnique();
    }
}
