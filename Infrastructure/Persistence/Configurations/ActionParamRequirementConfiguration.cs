using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class ActionParamRequirementConfiguration : IEntityTypeConfiguration<ActionParamRequirement>
{
    public void Configure(EntityTypeBuilder<ActionParamRequirement> builder)
    {
        builder.ToTable("action_param_requirement", t =>
        {
            t.HasCheckConstraint(
                "ck_action_param_requirement_value_kind",
                "value_kind IN ('CONSTANT','VARIABLE')");

            t.HasCheckConstraint(
                "ck_action_param_requirement_sort_order",
                "sort_order >= 0");
        });

        builder.HasKey(x => new { x.ActionId, x.ParamDefinitionId })
            .HasName("pk_action_param_requirement");

        builder.Property(x => x.IsRequired)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.ValueKind)
            .IsRequired();

        builder.Property(x => x.UiPrompt);

        builder.Property(x => x.ValidationRule)
            .HasColumnType("jsonb");

        builder.Property(x => x.SortOrder)
            .HasDefaultValue(0)
            .IsRequired();

        builder.HasIndex(x => new { x.ActionId, x.SortOrder })
            .IsUnique()
            .HasDatabaseName("uq_action_param_requirement_sort_order");

        builder.HasOne(x => x.Action)
            .WithMany(x => x.ActionParamRequirements)
            .HasForeignKey(x => x.ActionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_action_param_requirement_action");

        builder.HasOne(x => x.ParamDefinition)
            .WithMany(x => x.ActionParamRequirements)
            .HasForeignKey(x => x.ParamDefinitionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_action_param_requirement_param_definition");
    }
}
