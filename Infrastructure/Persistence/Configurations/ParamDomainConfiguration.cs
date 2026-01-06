using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class ParamDomainConfiguration : IEntityTypeConfiguration<ParamDomain>
{
    public void Configure(EntityTypeBuilder<ParamDomain> builder)
    {
        builder.ToTable("param_domain");

        builder.HasKey(x => x.ParamDomainId)
            .HasName("pk_param_domain");

        builder.Property(x => x.ParamDomainId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Code)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("uq_param_domain_code");
    }
}