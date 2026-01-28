using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы object.
/// </summary>
public sealed class ObjectConfiguration : IEntityTypeConfiguration<ObjectTable>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ObjectTable> builder)
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
            .IsRequired();

        builder.HasOne(x => x.ObjectType)
            .WithMany(x => x.Objects)
            .HasForeignKey(x => x.ObjectTypeId);

        builder.HasMany(x => x.LineEnds)
            .WithOne(x => x.Object)
            .HasForeignKey(x => x.ObjectId);

        builder.HasMany(x => x.InstructionRequests)
            .WithOne(x => x.Object)
            .HasForeignKey(x => x.ObjectId);
    }
}
