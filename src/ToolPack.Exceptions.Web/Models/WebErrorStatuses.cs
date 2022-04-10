namespace ToolPack.Exceptions.Web.Models
{
    using Grpc.Core;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Security.Authentication;
    using System.Threading.Tasks;
    using ToolPack.Exceptions.Base.Entities;

    /// <summary>
    /// Opiniated Error Statuses relating error types according to their meaning in the Web (HTTP and gRPC status codes).
    /// This also relates the Error Statuses with known types of Exceptions - both custom and generic.
    /// </summary>
    public static class WebErrorStatuses
    {
        /// <summary>Entity already exists.</summary>
        public static readonly WebErrorStatus AlreadyExists =
            new(StatusCode.AlreadyExists, HttpStatusCode.Conflict, "Entity already exists.");

        /// <summary>Argument is incorrect.</summary>
        public static readonly WebErrorStatus ArgumentError =
            new(StatusCode.InvalidArgument, HttpStatusCode.BadRequest, "Argument is incorrect.");

        /// <summary>Underlying dependency failed.</summary>
        public static readonly WebErrorStatus FailedDependency =
            new(StatusCode.Internal, HttpStatusCode.FailedDependency, "Underlying component on which the system depends failed.");

        /// <summary>Permission to operation is not granted.</summary>
        public static readonly WebErrorStatus ForbiddenPermission =
            new(StatusCode.PermissionDenied, HttpStatusCode.Forbidden, "Permission to operation is not granted.");

        /// <summary>An internal error in the system.</summary>
        public static readonly WebErrorStatus InternalError =
            new(StatusCode.Internal, HttpStatusCode.InternalServerError, "An internal error occurred.");

        /// <summary>An internal unknown error in the system.</summary>
        public static readonly WebErrorStatus InternalUnknownError =
            new(StatusCode.Unknown, HttpStatusCode.InternalServerError, "An internal unknown error occurred.");

        /// <summary>Entity not found.</summary>
        public static readonly WebErrorStatus NotFound =
            new(StatusCode.NotFound, HttpStatusCode.NotFound, "Entity not found.");

        /// <summary>Operation is not supported.</summary>
        public static readonly WebErrorStatus NotImplemented =
            new(StatusCode.Unimplemented, HttpStatusCode.NotImplemented, "Operation is not supported.");

        /// <summary>Precondition required to operation failed.</summary>
        public static readonly WebErrorStatus PreconditionFailed =
            new(StatusCode.FailedPrecondition, HttpStatusCode.BadRequest, "Precondition required to operation failed.");

        /// <summary>Operation was cancelled.</summary>
        public static readonly WebErrorStatus RequestCancelled =
            new(StatusCode.Cancelled, HttpStatusCode.BadRequest, "Operation was cancelled.");

        /// <summary>Operation timed out.</summary>
        public static readonly WebErrorStatus Timeout =
            new(StatusCode.DeadlineExceeded, HttpStatusCode.GatewayTimeout, "Operation timed out.");

        /// <summary>Required authentication is absent.</summary>
        public static readonly WebErrorStatus Unauthenticated =
            new(StatusCode.Unauthenticated, HttpStatusCode.Unauthorized, "Required authentication is absent.");

        /// <summary>Service is not available.</summary>
        public static readonly WebErrorStatus UnavailableError =
            new(StatusCode.Unavailable, HttpStatusCode.ServiceUnavailable, "Service is not available.");

        /// <summary>Gets a web error status according to the given exception type.</summary>
        /// <typeparam name="T">The exception type.</typeparam>
        /// <param name="exception">The exception.</param>
        /// <returns>An instance of WebErrorStatus, matching the given exception.</returns>
        public static WebErrorStatus GetFromException<T>(T exception)
            where T : Exception
        {
            return exception switch
            {
                CustomBaseException customException => GetFromCustomBaseException(customException),
                _ => GetFromSystemException(exception),
            };
        }

        private static WebErrorStatus GetFromCustomBaseException<T>(T exception)
            where T : CustomBaseException
        {
            return exception switch
            {
                AlreadyExistsException => AlreadyExists,
                ExternalComponentException => FailedDependency,
                NotFoundException => NotFound,
                ValidationFailedException => PreconditionFailed,
                _ => InternalError
            };
        }

        private static WebErrorStatus GetFromSystemException<T>(T exception)
            where T : Exception
        {
            return exception switch
            {
                ArgumentException => ArgumentError,
                AuthenticationException => Unauthenticated,
                HttpRequestException => UnavailableError,
                NotImplementedException => NotImplemented,
                TaskCanceledException => RequestCancelled,
                TimeoutException => Timeout,
                UnauthorizedAccessException => ForbiddenPermission,
                _ => InternalUnknownError
            };
        }
    }
}
