namespace ToolPack.Exceptions.UnitTests.Web.Services;

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics;
using ToolPack.Exceptions.Base.Entities;
using ToolPack.Exceptions.Web.Models;
using ToolPack.Exceptions.Web.Services;
using ToolPack.Exceptions.Web.Services.Implementations;

public class ProblemDetailsServiceTests
{
    private static Mock<ILogger<ProblemDetailsService>> LoggerMock => new();

    [Test]
    public void BuildProblemDetailsResponse_ExceptionIsNull_ReturnsDefault()
    {
        // Arrange
        var httpContextAccessor = GetHttpContextAccessorMock().Object;
        var expectedErrorStatus = WebErrorStatuses.InternalUnknownError;

        new ProblemDetails(Activity.Current?.Id ?? httpContextAccessor.HttpContext?.TraceIdentifier)
            .TrySerializeCamelCase(out var expectedProblemDetailsJson, out _);

        ProblemDetailsService problemDetailsService = new(httpContextAccessor, LoggerMock.Object);

        // Act
        var (problemDetailsJsonResult, errorStatus) = problemDetailsService.BuildProblemDetailsResponse((Exception)null);

        // Assert
        problemDetailsJsonResult.Should().Be(expectedProblemDetailsJson);
        errorStatus.Should().BeEquivalentTo(expectedErrorStatus);
    }

    [Test]
    public void BuildProblemDetailsResponse_ExceptionIsValid_ReturnsMatchingProblemDetailsString()
    {
        // Arrange
        var httpContextAccessorMock = GetHttpContextAccessorMock();

        CustomBaseException exception = new();
        ProblemDetails exceptionProblemDetails = new(exception, Activity.Current?.Id ?? httpContextAccessorMock.Object.HttpContext.TraceIdentifier);

        var expectedErrorStatus = ExceptionToWebErrorMap.GetFromException(exception);
        _ = exceptionProblemDetails.TrySerializeCamelCase(out string expectedProblemDetailsJson, out _);

        ProblemDetailsService problemDetailsService = new(httpContextAccessorMock.Object, LoggerMock.Object);

        // Act
        var (problemDetailsJsonResult, errorStatus) = problemDetailsService.BuildProblemDetailsResponse(exception);

        // Assert
        problemDetailsJsonResult.Should().Be(expectedProblemDetailsJson);
        errorStatus.Should().BeEquivalentTo(expectedErrorStatus);
    }

    private static Mock<IHttpContextAccessor> GetHttpContextAccessorMock()
    {
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(new DefaultHttpContext());

        return mockHttpContextAccessor;
    }
}
