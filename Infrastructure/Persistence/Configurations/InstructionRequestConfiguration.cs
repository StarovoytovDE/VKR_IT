using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы instruction_request.
/// </summary>
public sealed class InstructionRequestConfiguration : IEntityTypeConfiguration<InstructionRequest>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<InstructionRequest> builder)
    {
        builder.ToTable("instruction_request");

        builder.HasKey(x => x.InstructionRequestId)
            .HasName("pk_instruction_request");

        builder.Property(x => x.InstructionRequestId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.ObjectId)
            .IsRequired();

        builder.Property(x => x.ActionId)
            .IsRequired();

        builder.Property(x => x.CreatedByUserId)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.Comment);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.HasOne(x => x.Object)
            .WithMany(x => x.InstructionRequests)
            .HasForeignKey(x => x.ObjectId);

        builder.HasOne(x => x.Action)
            .WithMany(x => x.InstructionRequests)
            .HasForeignKey(x => x.ActionId);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany(x => x.InstructionRequestsCreated)
            .HasForeignKey(x => x.CreatedByUserId);

        builder.HasOne(x => x.InstructionResult)
            .WithOne(x => x.InstructionRequest)
            .HasForeignKey<InstructionResult>(x => x.InstructionRequestId);
    }
}
