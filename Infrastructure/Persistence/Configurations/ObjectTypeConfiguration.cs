using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class ObjectTypeConfiguration : IEntityTypeConfiguration<ObjectType>
{
    public void Configure(EntityTypeBuilder<ObjectType> builder)
    {
        builder.ToTable("object_type");

        builder.HasKey(x => x.ObjectTypeId)
            .HasName("pk_object_type");

        builder.Property(x => x.ObjectTypeId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Code)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("uq_object_type_code");
    }
}
