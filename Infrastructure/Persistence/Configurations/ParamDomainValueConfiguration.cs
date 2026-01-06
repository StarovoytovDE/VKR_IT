using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class ParamDomainValueConfiguration : IEntityTypeConfiguration<ParamDomainValue>
{
    public void Configure(EntityTypeBuilder<ParamDomainValue> builder)
    {
        builder.ToTable("param_domain_value", t =>
        {
            t.HasCheckConstraint(
                "ck_param_domain_value_sort_order", "sort_order >= 0");
        });

        builder.HasKey(x => new { x.ParamDomainId, x.ValueCode })
            .HasName("pk_param_domain_value");

        builder.Property(x => x.ValueName)
            .IsRequired();

        builder.Property(x => x.SortOrder)
            .HasDefaultValue(0)
            .IsRequired();

        builder.HasIndex(x => new { x.ParamDomainId, x.ValueName })
            .IsUnique()
            .HasDatabaseName("uq_param_domain_value_name");

        builder.HasOne(x => x.ParamDomain)
            .WithMany(x => x.ParamDomainValues)
            .HasForeignKey(x => x.ParamDomainId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_param_domain_value_domain");
    }
}
