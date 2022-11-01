namespace ToolPack.Exceptions.Web.Models;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using ToolPack.Exceptions.Base.Entities;

internal static class ExceptionToWebErrorMap
{
    private static readonly Dictionary<string, WebErrorStatus> MappedValues = new();

    static ExceptionToWebErrorMap()
    {
        AddOrReplace<AlreadyExistsException>(WebErrorStatuses.AlreadyExists);
        AddOrReplace<ExternalComponentException>(WebErrorStatuses.FailedDependency);
        AddOrReplace<NotFoundException>(WebErrorStatuses.NotFound);
        AddOrReplace<ValidationFailedException>(WebErrorStatuses.PreconditionFailed);
        AddOrReplace<CustomBaseException>(WebErrorStatuses.InternalError);
        AddOrReplace<ArgumentException>(WebErrorStatuses.ArgumentError);
        AddOrReplace<AuthenticationException>(WebErrorStatuses.Unauthenticated);
        AddOrReplace<HttpRequestException>(WebErrorStatuses.UnavailableError);
        AddOrReplace<NotImplementedException>(WebErrorStatuses.NotImplemented);
        AddOrReplace<TaskCanceledException>(WebErrorStatuses.RequestCancelled);
        AddOrReplace<TimeoutException>(WebErrorStatuses.Timeout);
        AddOrReplace<UnauthorizedAccessException>(WebErrorStatuses.ForbiddenPermission);
        AddOrReplace<Exception>(WebErrorStatuses.InternalUnknownError);
    }

    internal static void AddOrReplace<TException>(WebErrorStatus webErrorStatus)
        where TException : Exception
    {
        MappedValues[GetKey<TException>()] = webErrorStatus;
    }

    internal static WebErrorStatus GetFromException<TException>(TException exception)
        where TException : Exception
    {
        if (MappedValues.TryGetValue(exception.GetKey(), out var webErrorStatus))
            return webErrorStatus;
        else if (exception is CustomBaseException)
            webErrorStatus = MappedValues.GetValueOrDefault(GetKey<CustomBaseException>(), new());
        else
            webErrorStatus = MappedValues.GetValueOrDefault(GetKey<Exception>(), new());

        return webErrorStatus;
    }

    private static string GetKey<TException>()
        where TException : Exception
        => typeof(TException).AssemblyQualifiedName;

    private static string GetKey<TException>(this TException exception)
        where TException : Exception
        => exception.GetType().AssemblyQualifiedName;
}
