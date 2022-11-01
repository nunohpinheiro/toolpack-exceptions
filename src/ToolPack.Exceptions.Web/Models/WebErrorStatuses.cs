namespace ToolPack.Exceptions.Web.Models;

using Grpc.Core;
using System.Net;

/// <summary>
/// Opiniated Error Statuses relating error types according to their meaning in the Web (HTTP and gRPC status codes).
/// This also relates the Error Statuses with known types of Exceptions - both custom and generic.
/// </summary>
internal static class WebErrorStatuses
{
    internal static readonly WebErrorStatus AlreadyExists =
        new(StatusCode.AlreadyExists, HttpStatusCode.Conflict, "Entity already exists.");

    internal static readonly WebErrorStatus ArgumentError =
        new(StatusCode.InvalidArgument, HttpStatusCode.BadRequest, "Argument is incorrect.");

    internal static readonly WebErrorStatus FailedDependency =
        new(StatusCode.Internal, HttpStatusCode.FailedDependency, "Underlying component on which the system depends failed.");

    internal static readonly WebErrorStatus ForbiddenPermission =
        new(StatusCode.PermissionDenied, HttpStatusCode.Forbidden, "Permission to operation is not granted.");

    internal static readonly WebErrorStatus InternalError =
        new(StatusCode.Internal, HttpStatusCode.InternalServerError, "An internal error occurred.");

    internal static readonly WebErrorStatus InternalUnknownError =
        new(StatusCode.Unknown, HttpStatusCode.InternalServerError, "An internal unknown error occurred.");

    internal static readonly WebErrorStatus NotFound =
        new(StatusCode.NotFound, HttpStatusCode.NotFound, "Entity not found.");

    internal static readonly WebErrorStatus NotImplemented =
        new(StatusCode.Unimplemented, HttpStatusCode.NotImplemented, "Operation is not supported.");

    internal static readonly WebErrorStatus PreconditionFailed =
        new(StatusCode.FailedPrecondition, HttpStatusCode.BadRequest, "Precondition required to operation failed.");

    internal static readonly WebErrorStatus RequestCancelled =
        new(StatusCode.Cancelled, HttpStatusCode.BadRequest, "Operation was cancelled.");

    internal static readonly WebErrorStatus Timeout =
        new(StatusCode.DeadlineExceeded, HttpStatusCode.GatewayTimeout, "Operation timed out.");

    internal static readonly WebErrorStatus Unauthenticated =
        new(StatusCode.Unauthenticated, HttpStatusCode.Unauthorized, "Required authentication is absent.");

    internal static readonly WebErrorStatus UnavailableError =
        new(StatusCode.Unavailable, HttpStatusCode.ServiceUnavailable, "Service is not available.");
}
