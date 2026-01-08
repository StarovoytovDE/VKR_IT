using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Tests;

/// <summary>
/// Базовые проверки сборки EF Core модели.
/// </summary>
public class DbContextModelTests
{
    /// <summary>
    /// Проверяет, что модель контекста строится и содержит ключевую сущность связи линий и устройств.
    /// </summary>
    [Fact]
    public void Model_ShouldBuild_WithLineRzaDeviceLink()
    {
        var options = new DbContextOptionsBuilder<VkrItDbContext>()
            .UseInMemoryDatabase(nameof(Model_ShouldBuild_WithLineRzaDeviceLink))
            .Options;

        using var context = new VkrItDbContext(options);
        var entityType = context.Model.FindEntityType("Domain.Entities.LineRzaDeviceLink");

        Assert.NotNull(entityType);
        Assert.NotNull(entityType!.FindPrimaryKey());
    }
}