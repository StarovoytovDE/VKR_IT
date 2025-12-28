using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Object = Domain.Entities.Object;

namespace Infrastructure.Persistence.Configurations;

public sealed class ObjectConfiguration : IEntityTypeConfiguration<Object>
{
    public void Configure(EntityTypeBuilder<Object> builder)
    {
        builder.ToTable("object");

        builder.HasKey(x => x.ObjectId)
            .HasName("pk_object");

        builder.Property(x => x.ObjectId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.ObjectTypeId)
            .IsRequired();

        builder.Property(x => x.Uid)
            .IsRequired();

        builder.Property(x => x.DispatchName)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasIndex(x => x.Uid)
            .IsUnique()
            .HasDatabaseName("uq_object_uid");

        builder.HasIndex(x => new { x.ObjectTypeId, x.DispatchName })
            .IsUnique()
            .HasDatabaseName("uq_object_type_dispatch_name");

        builder.HasOne(x => x.ObjectType)
            .WithMany(x => x.Objects)
            .HasForeignKey(x => x.ObjectTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_object_object_type");
    }
}