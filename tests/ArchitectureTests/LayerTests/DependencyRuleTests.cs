using FluentAssertions;
using StarterKit.Domain.Entities;
using StarterKit.Infrastructure.Data.Persistence.Context;
using StarterKit.Infrastructure.External.Caching.Redis;
using StarterKit.UseCase.Handlers.Products.Command.Create;
using Xunit;

namespace ArchitectureTests.LayerTests;

public sealed class DependencyRuleTests
{
    [Fact]
    public void Domain_should_not_reference_outer_layers()
    {
        var references = GetReferencedAssemblyNames(typeof(Product).Assembly);

        references.Should().NotContain("StarterKit.Api");
        references.Should().NotContain("StarterKit.UseCase");
        references.Should().NotContain("StarterKit.Infrastructure.Data");
        references.Should().NotContain("StarterKit.Infrastructure.External");
    }

    [Fact]
    public void UseCase_should_only_reference_domain_not_api_or_infrastructure()
    {
        var references = GetReferencedAssemblyNames(typeof(CreateProductCommand).Assembly);

        references.Should().Contain("StarterKit.Domain");
        references.Should().NotContain("StarterKit.Api");
        references.Should().NotContain("StarterKit.Infrastructure.Data");
        references.Should().NotContain("StarterKit.Infrastructure.External");
    }

    [Fact]
    public void InfrastructureData_should_reference_domain_and_usecase_but_not_api_or_external()
    {
        var references = GetReferencedAssemblyNames(typeof(ApplicationDbContext).Assembly);

        references.Should().Contain("StarterKit.Domain");
        references.Should().Contain("StarterKit.UseCase");
        references.Should().NotContain("StarterKit.Api");
        references.Should().NotContain("StarterKit.Infrastructure.External");
    }

    [Fact]
    public void InfrastructureExternal_should_reference_usecase_but_not_api_or_data()
    {
        var references = GetReferencedAssemblyNames(typeof(RedisCacheService).Assembly);

        references.Should().Contain("StarterKit.UseCase");
        references.Should().NotContain("StarterKit.Api");
        references.Should().NotContain("StarterKit.Infrastructure.Data");
    }

    private static string[] GetReferencedAssemblyNames(System.Reflection.Assembly assembly) =>
        assembly.GetReferencedAssemblies()
            .Select(name => name.Name!)
            .Where(name => name.StartsWith("StarterKit", StringComparison.Ordinal))
            .OrderBy(name => name)
            .ToArray();
}
