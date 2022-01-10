namespace ToolPack.Exceptions.UnitTests.Web.Extensions
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using ToolPack.Exceptions.Web.Extensions;
    using ToolPack.Exceptions.Web.Services.Interfaces;

    public class DependencyInjectionExtensionsTests
    {
        private static IServiceCollection _servicesFixture => new ServiceCollection().AddLogging();

        [Test]
        public void AddToolPackExceptions_ExpectedServicesAreAdded()
        {
            // Act
            var servicesResult = _servicesFixture.AddToolPackExceptions();

            // Assert
            AssertExceptionServices(servicesResult);
        }

        [Test]
        public void AddToolPackExceptionsGrpc_ExpectedServicesAreAdded()
        {
            // Act
            var servicesResult = _servicesFixture.AddToolPackExceptionsGrpc();

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
}
