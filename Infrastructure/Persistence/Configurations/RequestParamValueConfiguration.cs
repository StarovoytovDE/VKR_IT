using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class RequestParamValueConfiguration : IEntityTypeConfiguration<RequestParamValue>
{
    public void Configure(EntityTypeBuilder<RequestParamValue> builder)
    {
        builder.ToTable("request_param_value", t =>
        {
            t.HasCheckConstraint(
                "ck_request_param_value_origin",
                "value_origin IN ('CONSTANT_OBJECT','USER_INPUT','SCADA','CALCULATED')");

            t.HasCheckConstraint(
                "ck_request_param_value_single_value",
                "num_nonnulls(value_bool, value_int_enum, value_decimal, value_text, value_json) <= 1");
        });

        builder.HasKey(x => new { x.InstructionRequestId, x.ParamDefinitionId })
            .HasName("pk_request_param_value");

        builder.Property(x => x.ValueOrigin)
            .IsRequired();

        builder.Property(x => x.ValueDecimal)
            .HasColumnType("numeric");

        builder.Property(x => x.ValueJson)
            .HasColumnType("jsonb");

        builder.HasOne(x => x.InstructionRequest)
            .WithMany(x => x.RequestParamValues)
            .HasForeignKey(x => x.InstructionRequestId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_request_param_value_request");

        builder.HasOne(x => x.ParamDefinition)
            .WithMany(x => x.RequestParamValues)
            .HasForeignKey(x => x.ParamDefinitionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_request_param_value_param_definition");
    }
}
