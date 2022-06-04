using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ToolPack.Exceptions.UnitTests")]
namespace ToolPack.Exceptions.Web.Models;

using Grpc.Core;
using System.Net;

/// <summary>Placeholder that relates HTTP and gRPC Status Codes with a common description.</summary>
public record WebErrorStatus
{
    internal StatusCode GrpcCode { get; init; }
    internal HttpStatusCode HttpCode { get; init; }
    internal string Description { get; init; }

    internal WebErrorStatus(
        StatusCode grpcCode,
        HttpStatusCode httpCode,
        string typeDescription)
    {
        GrpcCode = grpcCode;
        HttpCode = httpCode;
        Description = typeDescription;
    }
}
