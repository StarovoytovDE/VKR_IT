using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class ParamDefinitionConfiguration : IEntityTypeConfiguration<ParamDefinition>
{
    public void Configure(EntityTypeBuilder<ParamDefinition> builder)
    {
        builder.ToTable("param_definition", t =>
        {
            t.HasCheckConstraint(
                "ck_param_definition_value_type",
                "value_type IN ('bool','enum','int','decimal','text','json')");

            t.HasCheckConstraint(
                "ck_param_definition_domain",
                "((value_type = 'enum' AND param_domain_id IS NOT NULL) OR (value_type <> 'enum' AND param_domain_id IS NULL))");
        });

        builder.HasKey(x => x.ParamDefinitionId)
            .HasName("pk_param_definition");

        builder.Property(x => x.ParamDefinitionId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Code)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.ValueType)
            .IsRequired();

        builder.Property(x => x.Unit);
        builder.Property(x => x.Description);

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("uq_param_definition_code");

        builder.HasOne(x => x.ParamDomain)
            .WithMany(x => x.ParamDefinitions)
            .HasForeignKey(x => x.ParamDomainId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_param_definition_domain");
    }
}
