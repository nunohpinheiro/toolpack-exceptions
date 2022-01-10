namespace ToolPack.Exceptions.UnitTests.Web.Services
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Diagnostics;
    using ToolPack.Exceptions.Base.Entities;
    using ToolPack.Exceptions.Web.Extensions;
    using ToolPack.Exceptions.Web.Models;
    using ToolPack.Exceptions.Web.Services.Implementations;

    public class ProblemDetailsServiceTests
    {
        private static Mock<ILogger<ProblemDetailsService>> LoggerMock => new();

        [Test]
        public void BuildProblemDetailsResponse_ExceptionIsNull_ReturnsDefault()
        {
            // Arrange
            var expectedErrorStatus = WebErrorStatus.InternalUnknownError;
            var expectedResult = expectedErrorStatus.HttpCode.ToString();
            ProblemDetailsService problemDetailsService = new(GetHttpContextAccessorMock().Object, LoggerMock.Object);

            // Act
            var (result, errorStatus) = problemDetailsService.BuildProblemDetailsResponse(null);

            // Assert
            result.Should().Be(expectedResult);
            errorStatus.Should().BeEquivalentTo(expectedErrorStatus);
        }

        [Test]
        public void BuildProblemDetailsResponse_ExceptionIsValid_ReturnsMatchingProblemDetailsString()
        {
            // Arrange
            var httpContextAccessorMock = GetHttpContextAccessorMock();

            CustomBaseException exception = new();
            ProblemDetails exceptionProblemDetails = GetProblemDetailsFromException(exception, httpContextAccessorMock.Object, out WebErrorStatus expectedErrorStatus);
            _ = exceptionProblemDetails.TrySerializeCamelCase(out string expectedProblemDetailsJson, out _);

            ProblemDetailsService problemDetailsService = new(httpContextAccessorMock.Object, LoggerMock.Object);

            // Act
            var (result, errorStatus) = problemDetailsService.BuildProblemDetailsResponse(exception);

            // Assert
            result.Should().Be(expectedProblemDetailsJson);
            errorStatus.Should().BeEquivalentTo(expectedErrorStatus);
        }

        private static Mock<IHttpContextAccessor> GetHttpContextAccessorMock()
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(new DefaultHttpContext());

            return mockHttpContextAccessor;
        }

        private ProblemDetails GetProblemDetailsFromException(
            Exception exception,
            IHttpContextAccessor httpContextAccessor,
            out WebErrorStatus exceptionErrorStatus)
        {
            ProblemDetailsWebResponse problemDetailsWebResponse = new(exception, Activity.Current?.Id ?? httpContextAccessor?.HttpContext?.TraceIdentifier);

            exceptionErrorStatus = problemDetailsWebResponse.WebErrorStatus;

            return problemDetailsWebResponse.ProblemDetails;
        }
    }
}
