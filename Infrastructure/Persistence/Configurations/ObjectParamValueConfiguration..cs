using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class ObjectParamValueConfiguration : IEntityTypeConfiguration<ObjectParamValue>
{
    public void Configure(EntityTypeBuilder<ObjectParamValue> builder)
    {
        builder.ToTable("object_param_value", t =>
        {
            t.HasCheckConstraint(
                "ck_object_param_value_single_value",
                "num_nonnulls(value_bool, value_int_enum, value_decimal, value_text, value_json) <= 1");
        });

        builder.HasKey(x => new { x.ObjectId, x.ParamDefinitionId })
            .HasName("pk_object_param_value");

        builder.Property(x => x.ValueDecimal)
            .HasColumnType("numeric");

        builder.Property(x => x.ValueJson)
            .HasColumnType("jsonb");

        builder.Property(x => x.UpdatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasOne(x => x.Object)
            .WithMany(x => x.ObjectParamValues)
            .HasForeignKey(x => x.ObjectId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_object_param_value_object");

        builder.HasOne(x => x.ParamDefinition)
            .WithMany(x => x.ObjectParamValues)
            .HasForeignKey(x => x.ParamDefinitionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_object_param_value_param_definition");

        builder.HasOne(x => x.UpdatedByUser)
            .WithMany(x => x.ObjectParamValuesUpdated)
            .HasForeignKey(x => x.UpdatedByUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_object_param_value_updated_by");
    }
}