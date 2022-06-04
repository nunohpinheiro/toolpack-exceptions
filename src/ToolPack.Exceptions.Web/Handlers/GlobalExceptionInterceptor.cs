namespace ToolPack.Exceptions.Web.Handlers;

using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ToolPack.Exceptions.Web.Services.Interfaces;

/// <summary>
/// Interceptor to handle exceptions in gRPC services globally.
/// </summary>
internal class GlobalExceptionInterceptor : Interceptor
{
    private readonly ILogger<GlobalExceptionInterceptor> _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionInterceptor(
        ILogger<GlobalExceptionInterceptor> logger,
        IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
    }

    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("Incoming request in GlobalExceptionInterceptor - client streaming server handler.");
        try
        {
            return await continuation(requestStream, context);
        }
        catch (Exception ex)
        {
            throw HandleException(context, ex);
        }
    }

    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("Incoming request in GlobalExceptionInterceptor - duplex streaming server handler.");
        try
        {
            await continuation(requestStream, responseStream, context);
        }
        catch (Exception ex)
        {
            throw HandleException(context, ex);
        }
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("Incoming request in GlobalExceptionInterceptor - server streaming server handler.");
        try
        {
            await continuation(request, responseStream, context);
        }
        catch (Exception ex)
        {
            throw HandleException(context, ex);
        }
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("Incoming request in GlobalExceptionInterceptor - unary server handler.");
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            throw HandleException(context, ex);
        }
    }

    private RpcException HandleException(ServerCallContext context, Exception ex)
    {
        _logger.LogError(
                "An exception was caught by the GlobalExceptionInterceptor. Context method: {ContextMethod} | Exception: {Exception}",
                context.Method,
                ex);

        return GetProblemDetailsRpcException(ex);
    }

    private RpcException GetProblemDetailsRpcException(Exception ex)
    {
        var (problemDetailsResponse, errorStatus) = _problemDetailsService.BuildProblemDetailsResponse(ex);

        _logger.LogInformation(
            "A ProblemDetails response was built with error status. ErrorStatus: {ErrorStatus} | ProblemDetails: {ProblemDetails}",
            errorStatus,
            problemDetailsResponse);

        return new RpcException(new Status(errorStatus.GrpcCode, problemDetailsResponse), problemDetailsResponse);
    }
}
