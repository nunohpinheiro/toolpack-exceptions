namespace ToolPack.Exceptions.Web.Services.Implementations;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using ToolPack.Exceptions.Web.Extensions;
using ToolPack.Exceptions.Web.Models;
using ToolPack.Exceptions.Web.Services.Interfaces;

internal class ProblemDetailsService : IProblemDetailsService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ProblemDetailsService> _logger;

    public ProblemDetailsService(
        IHttpContextAccessor httpContextAccessor,
        ILogger<ProblemDetailsService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public (string, WebErrorStatus) BuildProblemDetailsResponse(Exception exception)
    {
        if (exception is null)
        {
            _logger.LogWarning("A null exception was used to build a response with ProblemDetails. A default will be returned.");
            return (WebErrorStatuses.InternalUnknownError.HttpCode.ToString(), WebErrorStatuses.InternalUnknownError);
        }

        return GetProblemDetailsWebErrorResponse(exception);
    }

    private (string, WebErrorStatus) GetProblemDetailsWebErrorResponse(Exception exception)
    {
        var traceId = Activity.Current?.Id ?? _httpContextAccessor?.HttpContext?.TraceIdentifier;

        ProblemDetailsWebResponse problemDetailsResponse = new(exception, traceId);

        if (TrySerializeProblemDetails(problemDetailsResponse.ProblemDetails, out var problemDetailsResponseJson))
            return (problemDetailsResponseJson, problemDetailsResponse.WebErrorStatus);

        return GetDefaultProblemDetailsWebErrorResponse(traceId);
    }

    private (string, WebErrorStatus) GetDefaultProblemDetailsWebErrorResponse(string traceId)
    {
        TrySerializeProblemDetails(new(traceId), out var problemDetailsDefaultResponseJson);

        return (problemDetailsDefaultResponseJson, WebErrorStatuses.InternalUnknownError);
    }

    private bool TrySerializeProblemDetails(
        ProblemDetails problemDetails,
        out string outputJson)
    {
        outputJson = null;
        Exception serializationException = null;

        if (problemDetails?.TrySerializeCamelCase(out outputJson, out serializationException) is true
            && outputJson is not null)
        {
            return true;
        }

        _logger.LogError(
                "ProblemDetails instance was not serialized successfully. ProblemDetails: {ProblemDetails} | Serialization Exception: {SerializationException}",
                problemDetails,
                serializationException);
        return false;
    }
}
