namespace ToolPack.Exceptions.UnitTests.Web.DependencyInjection;

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ToolPack.Exceptions.Web.DependencyInjection;
using ToolPack.Exceptions.Web.Services.Interfaces;

public class DependencyInjectionExtensionsTests
{
    private static IServiceCollection ServicesFixture => new ServiceCollection().AddLogging();

    // TODO: Add tests including options (adding/overriding exception mappings)

    [Test]
    public void AddToolPackExceptions_ExpectedServicesAreAdded()
    {
        // Act
        var servicesResult = ServicesFixture.AddToolPackExceptions();

        // Assert
        AssertExceptionServices(servicesResult);
    }

    [Test]
    public void AddToolPackExceptionsGrpc_ExpectedServicesAreAdded()
    {
        // Act
        var servicesResult = ServicesFixture.AddToolPackExceptionsGrpc();

        // Assert
        AssertExceptionServices(servicesResult);
    }

    private static void AssertExceptionServices(IServiceCollection services)
    {
        var problemDetailsSvc = services.BuildServiceProvider().GetService<IProblemDetailsService>();
        var httpContextSvc = services.BuildServiceProvider().GetService<IHttpContextAccessor>();

        problemDetailsSvc.Should().NotBeNull();
        httpContextSvc.Should().NotBeNull();
    }
}
