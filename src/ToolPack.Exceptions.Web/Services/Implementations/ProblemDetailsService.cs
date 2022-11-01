namespace ToolPack.Exceptions.Web.Services.Implementations;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
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

    public (string, WebErrorStatus) BuildProblemDetailsResponse<T>(T exception)
        where T : Exception
    {
        var traceId = Activity.Current?.Id ?? _httpContextAccessor?.HttpContext?.TraceIdentifier;
        return GetProblemDetailsWebErrorResponse(exception, traceId);
    }

    private (string, WebErrorStatus) GetProblemDetailsWebErrorResponse<T>(T exception, string traceId)
        where T : Exception
    {
        if (exception is null)
        {
            _logger.LogWarning("A null exception was used to build a response with ProblemDetails. A default will be returned.");
            return GetDefaultProblemDetailsWebErrorResponse(traceId);
        }

        if (TrySerializeProblemDetails(new(exception, traceId), out var problemDetailsResponseJson))
            return (problemDetailsResponseJson, ExceptionToWebErrorMap.GetFromException(exception));

        return GetDefaultProblemDetailsWebErrorResponse(traceId);
    }

    private (string, WebErrorStatus) GetDefaultProblemDetailsWebErrorResponse(string traceId)
    {
        TrySerializeProblemDetails(new(traceId), out var problemDetailsDefaultResponseJson);

        return (problemDetailsDefaultResponseJson, new());
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
