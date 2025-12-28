using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Action = Domain.Entities.Action;
using Object = Domain.Entities.Object;

namespace Infrastructure.Persistence;

public sealed class VkrItDbContext(DbContextOptions<VkrItDbContext> options)
                                                        : DbContext(options)
{
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<AppRole> AppRoles => Set<AppRole>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<ObjectType> ObjectTypes => Set<ObjectType>();
    public DbSet<Object> Objects => Set<Object>();
    public DbSet<Action> Actions => Set<Action>();
    public DbSet<ActionVersion> ActionVersions => Set<ActionVersion>();
    public DbSet<ParamDomain> ParamDomains => Set<ParamDomain>();
    public DbSet<ParamDomainValue> ParamDomainValues => Set<ParamDomainValue>();
    public DbSet<ParamDefinition> ParamDefinitions => Set<ParamDefinition>();
    public DbSet<ActionParamRequirement> ActionParamRequirements => Set<ActionParamRequirement>();
    public DbSet<ObjectParamValue> ObjectParamValues => Set<ObjectParamValue>();
    public DbSet<InstructionRequest> InstructionRequests => Set<InstructionRequest>();
    public DbSet<RequestParamValue> RequestParamValues => Set<RequestParamValue>();
    public DbSet<InstructionResult> InstructionResults => Set<InstructionResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VkrItDbContext).Assembly);
    }
}

