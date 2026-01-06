using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class InstructionRequestConfiguration : IEntityTypeConfiguration<InstructionRequest>
{
    public void Configure(EntityTypeBuilder<InstructionRequest> builder)
    {
        builder.ToTable("instruction_request", t =>
        {
            t.HasCheckConstraint(
                "ck_instruction_request_status",
                "status IN ('DRAFT','CALCULATED','SAVED','ERROR')");
        });

        builder.HasKey(x => x.InstructionRequestId)
            .HasName("pk_instruction_request");

        builder.Property(x => x.InstructionRequestId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.Comment);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasIndex(x => x.CreatedAt)
            .IsDescending()
            .HasDatabaseName("ix_ir_created_at_desc");

        builder.HasIndex(x => new { x.ObjectId, x.CreatedAt })
            .IsDescending(false, true)
            .HasDatabaseName("ix_ir_object_created_at_desc");

        builder.HasIndex(x => x.CreatedAt)
            .IsDescending()
            .HasFilter("status = 'ERROR'")
            .HasDatabaseName("ix_ir_error_created_at_desc");

        builder.HasOne(x => x.Object)
            .WithMany(x => x.InstructionRequests)
            .HasForeignKey(x => x.ObjectId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_instruction_request_object");

        builder.HasOne(x => x.Action)
            .WithMany(x => x.InstructionRequests)
            .HasForeignKey(x => x.ActionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_instruction_request_action");

        builder.HasOne(x => x.CreatedByUser)
            .WithMany(x => x.InstructionRequestsCreated)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_instruction_request_created_by");

        builder.HasOne(x => x.InstructionResult)
            .WithOne(x => x.InstructionRequest)
            .HasForeignKey<InstructionResult>(x => x.InstructionRequestId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_instruction_result_request");
    }
}