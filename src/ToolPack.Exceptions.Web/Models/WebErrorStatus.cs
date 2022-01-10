using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ToolPack.Exceptions.UnitTests")]
namespace ToolPack.Exceptions.Web.Models
{
    using Grpc.Core;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Security.Authentication;
    using System.Threading.Tasks;
    using ToolPack.Exceptions.Base.Entities;

    /// <summary>Error Status that relates errors according to their meaning in the Web (HTTP and RPC status codes).</summary>
    internal record WebErrorStatus
    {
        // TODO: Expose these publicly
        internal static readonly WebErrorStatus AlreadyExists = new(StatusCode.AlreadyExists, HttpStatusCode.Conflict, ErrorType.AlreadyExists);
        internal static readonly WebErrorStatus ArgumentError = new(StatusCode.InvalidArgument, HttpStatusCode.BadRequest, ErrorType.BadRequest);
        internal static readonly WebErrorStatus ArgumentOutOfRange = new(StatusCode.OutOfRange, HttpStatusCode.BadRequest, ErrorType.BadRequest);
        internal static readonly WebErrorStatus ForbiddenPermission = new(StatusCode.PermissionDenied, HttpStatusCode.Forbidden, ErrorType.Forbidden);
        internal static readonly WebErrorStatus InternalError = new(StatusCode.Internal, HttpStatusCode.InternalServerError, ErrorType.InternalServerError);
        internal static readonly WebErrorStatus InternalUnknownError = new(StatusCode.Unknown, HttpStatusCode.InternalServerError, ErrorType.InternalServerError);
        internal static readonly WebErrorStatus NotFound = new(StatusCode.NotFound, HttpStatusCode.NotFound, ErrorType.NotFound);
        internal static readonly WebErrorStatus NotImplemented = new(StatusCode.Unimplemented, HttpStatusCode.NotImplemented, ErrorType.NotImplemented);
        internal static readonly WebErrorStatus PreconditionFailed = new(StatusCode.FailedPrecondition, HttpStatusCode.BadRequest, ErrorType.BadRequest);
        internal static readonly WebErrorStatus RequestCancelled = new(StatusCode.Cancelled, HttpStatusCode.BadRequest, ErrorType.BadRequest);
        internal static readonly WebErrorStatus Timeout = new(StatusCode.DeadlineExceeded, HttpStatusCode.GatewayTimeout, ErrorType.InternalServerError);
        internal static readonly WebErrorStatus Unauthenticated = new(StatusCode.Unauthenticated, HttpStatusCode.Unauthorized, ErrorType.Unauthorized);
        internal static readonly WebErrorStatus UnavailableError = new(StatusCode.Unavailable, HttpStatusCode.ServiceUnavailable, ErrorType.ServiceUnavailable);

        public StatusCode GrpcCode { get; init; }
        public HttpStatusCode HttpCode { get; init; }
        public string TypeDescription { get; init; }

        private WebErrorStatus(
            StatusCode grpcCode,
            HttpStatusCode httpCode,
            string typeDescription)
        {
            GrpcCode = grpcCode;
            HttpCode = httpCode;
            TypeDescription = typeDescription;
        }

        /// <summary>Gets the error status according to the given exception type.</summary>
        /// <typeparam name="T">The exception type.</typeparam>
        /// <param name="exception">The exception.</param>
        /// <returns>An instance of WebErrorStatus, matching the given exception.</returns>
        internal static WebErrorStatus GetFromException<T>(T exception)
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
                ArgumentOutOfRangeException => ArgumentOutOfRange,
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
