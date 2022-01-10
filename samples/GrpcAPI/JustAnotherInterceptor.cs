namespace GrpcAPI
{
    using Grpc.Core;
    using Grpc.Core.Interceptors;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public class JustAnotherInterceptor : Interceptor
    {
        private readonly ILogger<JustAnotherInterceptor> _logger;

        public JustAnotherInterceptor(
            ILogger<JustAnotherInterceptor> logger)
        {
            _logger = logger;
        }

        public async override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
            IAsyncStreamReader<TRequest> requestStream,
            ServerCallContext context,
            ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation("JUST ANOTHER INTERCEPTOR - client streaming server handler.");
            return await continuation(requestStream, context);
        }

        public async override Task DuplexStreamingServerHandler<TRequest, TResponse>(
            IAsyncStreamReader<TRequest> requestStream,
            IServerStreamWriter<TResponse> responseStream,
            ServerCallContext context,
            DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation("JUST ANOTHER INTERCEPTOR - duplex streaming server handler.");
            await continuation(requestStream, responseStream, context);
        }

        public async override Task ServerStreamingServerHandler<TRequest, TResponse>(
            TRequest request,
            IServerStreamWriter<TResponse> responseStream,
            ServerCallContext context,
            ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation("JUST ANOTHER INTERCEPTOR - server streaming server handler.");
            await continuation(request, responseStream, context);
        }

        public async override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation("JUST ANOTHER INTERCEPTOR - unary server handler.");
            return await continuation(request, context);
        }
    }
}
