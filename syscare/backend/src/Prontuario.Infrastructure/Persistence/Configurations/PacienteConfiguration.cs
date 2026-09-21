using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prontuario.Domain.Entities;

namespace Prontuario.Infrastructure.Persistence.Configurations;

public class PacienteConfiguration : IEntityTypeConfiguration<Paciente>
{
    public void Configure(EntityTypeBuilder<Paciente> builder)
    {
        builder.ToTable("pacientes");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("id");
        builder.Property(p => p.Nome).HasColumnName("nome").IsRequired();
        builder.Property(p => p.DataNascimento).HasColumnName("data_nascimento");
        builder.Property(p => p.Sexo).HasColumnName("sexo");
        builder.Property(p => p.EstadoCivil).HasColumnName("estado_civil");
        builder.Property(p => p.Ocupacao).HasColumnName("ocupacao");
        builder.Property(p => p.NumeroFilhos).HasColumnName("numero_filhos");
        builder.Property(p => p.Religiao).HasColumnName("religiao");
        builder.Property(p => p.Telefone).HasColumnName("telefone");
        builder.Property(p => p.Endereco).HasColumnName("endereco");
        builder.Property(p => p.Numero).HasColumnName("numero");
        builder.Property(p => p.Bairro).HasColumnName("bairro");
        builder.Property(p => p.Escolaridade).HasColumnName("escolaridade");
        builder.Property(p => p.UbsReferencia).HasColumnName("ubs_referencia");
        builder.Property(p => p.ConvenioParticular).HasColumnName("convenio_particular");
        builder.Property(p => p.PassaPoliclinica).HasColumnName("passa_policlinica");
        builder.Property(p => p.Foto).HasColumnName("foto");
        builder.Property(p => p.CriadoEm).HasColumnName("criado_em");
        builder.Property(p => p.Ativo).HasColumnName("ativo");

        builder.HasOne(p => p.AvaliacaoSaude)
            .WithOne(a => a.Paciente)
            .HasForeignKey<AvaliacaoSaude>(a => a.PacienteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.ExamesFisicos)
            .WithOne(e => e.Paciente)
            .HasForeignKey(e => e.PacienteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Feridas)
            .WithOne(f => f.Paciente)
            .HasForeignKey(f => f.PacienteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Diagnosticos)
            .WithOne(d => d.Paciente)
            .HasForeignKey(d => d.PacienteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Prescricoes)
            .WithOne(pr => pr.Paciente)
            .HasForeignKey(pr => pr.PacienteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Agendamentos)
            .WithOne(ag => ag.Paciente)
            .HasForeignKey(ag => ag.PacienteId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
