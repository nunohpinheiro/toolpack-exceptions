namespace ToolPack.Exceptions.Web.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using ToolPack.Exceptions.Base.Entities;

/// <summary>
/// Problem Details entity that extends the one in AspNetCore.Mvc (https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.problemdetails).
/// Used to return application problems, using the RFC 7807 (https://tools.ietf.org/html/rfc7807).
/// </summary>
public class ProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
{
    /// <summary>Gets or sets additional errors related with the Problem Details.</summary>
    public IDictionary<string, string[]> Errors { get; private set; }

    /// <summary>Tracing identifier related with the context of these ProblemDetails.</summary>
    public string TraceId { get; init; }

    /// <summary>Creates a ProblemDetails instance with properties derived from a given exception.</summary>
    /// <param name="exception">Exception from which the ProblemDetails properties are filled.</param>
    public ProblemDetails(Exception exception)
    {
        var exceptionErrorStatus = ExceptionToWebErrorMap.GetFromException(exception);

        Detail = exception?.Message;
        Instance = exceptionErrorStatus.Description;
        Status = exceptionErrorStatus.HttpCode;
        Title = exceptionErrorStatus.HttpCode.ToString();
        Type = exceptionErrorStatus.Description;

        SetErrors(exception);
    }

    /// <summary>Creates a ProblemDetails instance with properties derived from a given exception.</summary>
    /// <param name="exception">Exception from which the ProblemDetails properties are filled.</param>
    /// <param name="traceId">Tracing identifier related with the context of this ProblemDetails.</param>
    public ProblemDetails(Exception exception, string traceId)
        : this(exception)
    {
        TraceId = traceId;
    }

    /// <summary>Creates a default ProblemDetails instance.</summary>
    /// <param name="traceId">Tracing identifier related with the context of these ProblemDetails.</param>
    public ProblemDetails(string traceId)
    {
        var problemDetailsStatus = WebErrorStatuses.InternalUnknownError;

        Detail = problemDetailsStatus.HttpCode.ToString();
        Status = problemDetailsStatus.HttpCode;
        Title = problemDetailsStatus.HttpCode.ToString();
        TraceId = traceId;
        Type = problemDetailsStatus.Description;
    }

    private static string FilterExceptionName<TException>(TException exception)
    {
        var exceptionNames = exception.GetType().Name.Split("Exception");
        return string.Join(" ", exceptionNames).Trim();
    }

    // TODO: Add tests to ProblemDetails construction - more domain driven now

    private void SetErrors(Exception exception)
    {
        if (exception is null)
            return;

        if (exception is AggregateException aggregateException)
            SetErrorsFromAggregateException(aggregateException);

        else if ((exception is ValidationFailedException validationException) && validationException.Errors.Any())
            Errors = validationException.Errors;

        else if (exception.InnerException is not null)
            SetErrorsFromInnerException(exception.InnerException);
    }

    private void SetErrorsFromAggregateException(AggregateException exception)
    {
        var failureGroups = exception?.InnerExceptions?.GroupBy(
                                e => FilterExceptionName(e),
                                e => e.Message);

        if (failureGroups?.Any() is not true)
            return;

        Errors = new Dictionary<string, string[]>();

        foreach (var failureGroup in failureGroups)
        {
            var propertyName = failureGroup.Key;
            var propertyFailures = failureGroup.ToArray();

            Errors.Add(propertyName, propertyFailures);
        }
    }

    private void SetErrorsFromInnerException(Exception innerException)
    {
        Errors = new Dictionary<string, string[]>
        {
            {
                FilterExceptionName(innerException),
                new string[1] { innerException.Message }
            }
        };
    }
}
