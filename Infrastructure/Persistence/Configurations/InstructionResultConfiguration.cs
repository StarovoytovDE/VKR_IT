using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class InstructionResultConfiguration : IEntityTypeConfiguration<InstructionResult>
{
    public void Configure(EntityTypeBuilder<InstructionResult> builder)
    {
        builder.ToTable("instruction_result");

        builder.HasKey(x => x.InstructionRequestId)
            .HasName("pk_instruction_result");

        builder.Property(x => x.GeneratedText)
            .IsRequired();

        builder.Property(x => x.GeneratedAt)
            .HasColumnType("timestamptz")
            .IsRequired();
    }
}
