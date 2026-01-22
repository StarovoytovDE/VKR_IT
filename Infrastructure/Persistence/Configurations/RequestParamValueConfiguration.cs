using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы request_param_value (новая версия по диаграмме).
/// </summary>
public sealed class RequestParamValueConfiguration : IEntityTypeConfiguration<RequestParamValue>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<RequestParamValue> builder)
    {
        builder.ToTable("request_param_value");

        builder.HasKey(x => x.RequestParamValueId)
            .HasName("pk_request_param_value");

        builder.Property(x => x.RequestParamValueId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.DeviceId)
            .IsRequired();

        builder.Property(x => x.InstructionRequestId)
            .IsRequired();

        builder.HasOne(x => x.Device)
            .WithMany(x => x.RequestParamValues)
            .HasForeignKey(x => x.DeviceId);

        builder.HasOne(x => x.InstructionRequest)
            .WithMany(x => x.RequestParamValues)
            .HasForeignKey(x => x.InstructionRequestId);

        builder.HasOne(x => x.Dfz)
            .WithMany()
            .HasForeignKey(x => x.DfzId);

        builder.HasOne(x => x.Dzl)
            .WithMany()
            .HasForeignKey(x => x.DzlId);

        builder.HasOne(x => x.Dz)
            .WithMany()
            .HasForeignKey(x => x.DzId);

        builder.HasOne(x => x.Oapv)
            .WithMany()
            .HasForeignKey(x => x.OapvId);

        builder.HasOne(x => x.Tapv)
            .WithMany()
            .HasForeignKey(x => x.TapvId);

        builder.HasOne(x => x.MtzBusbar)
            .WithMany()
            .HasForeignKey(x => x.MtzBusbarId);
    }
}
