using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prontuario.Domain.Entities;

namespace Prontuario.Infrastructure.Persistence.Configurations;

public class AgendamentoConfiguration : IEntityTypeConfiguration<Agendamento>
{
    public void Configure(EntityTypeBuilder<Agendamento> builder)
    {
        builder.ToTable("agendamentos");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.PacienteId).HasColumnName("paciente_id");
        builder.Property(a => a.NomePacienteAvulso).HasColumnName("nome_paciente_avulso");
        builder.Property(a => a.TelefoneAvulso).HasColumnName("telefone_avulso");
        builder.Property(a => a.Data).HasColumnName("data").IsRequired();
        builder.Property(a => a.Hora).HasColumnName("hora").IsRequired();
        builder.Property(a => a.DuracaoMin).HasColumnName("duracao_min");
        builder.Property(a => a.Procedimento).HasColumnName("procedimento");
        builder.Property(a => a.Status).HasColumnName("status").IsRequired();
        builder.Property(a => a.Observacoes).HasColumnName("observacoes");
        builder.Property(a => a.CriadoEm).HasColumnName("criado_em");

        builder.HasIndex(a => a.Data);
        builder.HasIndex(a => a.PacienteId);
    }
}
