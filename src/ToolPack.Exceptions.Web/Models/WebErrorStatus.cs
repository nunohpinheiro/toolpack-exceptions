namespace ToolPack.Exceptions.Web.Models;

using Grpc.Core;
using System.Net;

/// <summary>Placeholder that relates an HTTP Status Code, a gRPC Status Code and a common description.
/// Useful to create common error responses for Web APIs.</summary>
public struct WebErrorStatus
{
    internal StatusCode GrpcCode { get; private init; } = WebErrorStatuses.InternalUnknownError.GrpcCode;
    internal int HttpCode { get; private init; } = WebErrorStatuses.InternalUnknownError.HttpCode;
    internal string Description { get; private init; } = WebErrorStatuses.InternalUnknownError.Description;

    /// <summary>Initializes a new default instance of WebErrorStatus, with:
    ///     GrpcCode = 2 Unknown,
    ///     HttpCode = 500 InternalServerError,
    ///     Description = "An internal unknown error occurred.".</summary>
    public WebErrorStatus() { }

    /// <summary>Initializes a new instance of WebErrorStatus.</summary>
    /// <param name="grpcCode">The gRPC Status Code.</param>
    /// <param name="httpCode">The HTTP Status Code.</param>
    /// <param name="typeDescription">The common description to be used along both statuses.</param>
    public WebErrorStatus(
        StatusCode grpcCode,
        HttpStatusCode httpCode,
        string typeDescription)
    {
        GrpcCode = grpcCode;
        HttpCode = (int)httpCode;
        Description = typeDescription;
    }

    /// <summary>Initializes a new instance of WebErrorStatus.</summary>
    /// <param name="grpcCode">The gRPC Status Code.</param>
    /// <param name="httpCode">The HTTP Status Code.</param>
    /// <param name="typeDescription">The common description to be used along both statuses.</param>
    public WebErrorStatus(
        StatusCode grpcCode,
        int httpCode,
        string typeDescription)
    {
        GrpcCode = grpcCode;
        HttpCode = httpCode;
        Description = typeDescription;
    }
}
