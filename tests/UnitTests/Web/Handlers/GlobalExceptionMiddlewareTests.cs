namespace ToolPack.Exceptions.UnitTests.Web.Handlers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;
    using ToolPack.Exceptions.Web.Handlers;
    using ToolPack.Exceptions.Web.Models;
    using ToolPack.Exceptions.Web.Services.Interfaces;

    public class GlobalExceptionMiddlewareTests
    {
        private static Mock<HttpContext> HttpContextMock => new();
        private static Mock<ILogger<GlobalExceptionMiddleware>> LoggerMock => new();
        private static Mock<IProblemDetailsService> ProblemDetailsSvcMock => new();

        [Test]
        public void InvokeAsync_RequestDelegateNotThrowsException_ProblemDetailsServiceIsNotCalled()
        {
            // Arrange
            var requestDelegate = new RequestDelegate(innerContext => Task.FromResult(0));
            var exceptionsMiddleware = new GlobalExceptionMiddleware(LoggerMock.Object, requestDelegate);

            var problemDetailsSvcMock = ProblemDetailsSvcMock;

            WebErrorStatus errorStatus = It.IsAny<WebErrorStatus>();

            // Act
            Func<Task> act = async () => await exceptionsMiddleware.InvokeAsync(HttpContextMock.Object, problemDetailsSvcMock.Object);

            // Assert
            act.Should().NotThrow();
            problemDetailsSvcMock.Verify(
                x => x.BuildProblemDetailsResponse(It.IsAny<Exception>()),
                Times.Never);
        }

        [Test]
        public void InvokeAsync_RequestDelegateThrowsException_ProblemDetailsServiceIsCalled()
        {
            // Arrange
            var exception = new Exception("sample message");
            var requestDelegate = new RequestDelegate(innerContext => throw exception);
            var exceptionsMiddleware = new GlobalExceptionMiddleware(LoggerMock.Object, requestDelegate);

            var problemDetailsSvcMock = ProblemDetailsSvcMock;

            WebErrorStatus errorStatus = It.IsAny<WebErrorStatus>();

            // Act
            Func<Task> act = async () => await exceptionsMiddleware.InvokeAsync(HttpContextMock.Object, problemDetailsSvcMock.Object);

            // Assert
            act.Should().NotThrow();
            problemDetailsSvcMock.Verify(
                x => x.BuildProblemDetailsResponse(It.IsAny<Exception>()),
                Times.Once);
        }
    }
}
