namespace GrpcAPI
{
    using FluentValidation.Results;
    using Grpc.Core;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ToolPack.Exceptions.Base.Entities;
    using ToolPack.Exceptions.Base.Guard;

    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<Response> GetAlreadyExists(Request request, ServerCallContext context)
        {
            _logger.LogInformation($"gRPC unary request");

            _logger.LogInformation("Throwing AlreadyExistsException");

            ThrowWhen.ConditionFails(false, new AlreadyExistsException());

            return Task.FromResult(new Response());
        }

        public override Task GetAlreadyExistsStream(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            _logger.LogInformation($"gRPC stream request");

            _logger.LogInformation("Throwing AlreadyExistsException");

            ThrowWhen.ConditionFails(false, new AlreadyExistsException());
            return Task.CompletedTask;
        }

        public override Task<Response> GetCustomBaseException(Request request, ServerCallContext context)
        {
            _logger.LogInformation($"gRPC unary request");

            _logger.LogInformation("Throwing CustomBaseException");

            throw new CustomBaseException();
        }

        public override Task GetCustomBaseExceptionStream(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            _logger.LogInformation($"gRPC stream request");

            _logger.LogInformation("Throwing CustomBaseException");

            throw new CustomBaseException();
        }

        public override Task<Response> GetNotFound(Request request, ServerCallContext context)
        {
            _logger.LogInformation($"gRPC unary request");

            _logger.LogInformation("Throwing NotFoundException");

            ThrowWhen.ArgumentNullThrowNotFound<string>(null, "argument description");

            return Task.FromResult(new Response());
        }

        public override Task GetNotFoundStream(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            _logger.LogInformation($"gRPC stream request");

            _logger.LogInformation("Throwing NotFoundException");

            ThrowWhen.ArgumentNullThrowNotFound<string>(null, "argument description");
            return Task.CompletedTask;
        }

        public override Task<Response> GetThirdPartyException(Request request, ServerCallContext context)
        {
            _logger.LogInformation($"gRPC unary request");

            _logger.LogInformation("Throwing ExternalComponentException");

            ThrowWhen.ConditionFails(false, new ExternalComponentException());

            return Task.FromResult(new Response());
        }

        public override Task GetThirdPartyExceptionStream(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            _logger.LogInformation($"gRPC stream request");

            _logger.LogInformation("Throwing ExternalComponentException");

            ThrowWhen.ConditionFails(false, new ExternalComponentException());
            return Task.CompletedTask;
        }

        public override Task<Response> GetValidationFailedException(Request request, ServerCallContext context)
        {
            _logger.LogInformation($"gRPC unary request");

            _logger.LogInformation("Throwing ValidationFailedException");

            throw new ValidationFailedException(
                new List<ValidationFailure>()
                {
                    new("Prop1 Name", "Prop1 error message"),
                    new("Prop2 Name", "Prop2 error message")
                });
        }

        public override Task GetValidationFailedExceptionStream(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            _logger.LogInformation($"gRPC stream request");

            _logger.LogInformation("Throwing ValidationFailedException");

            throw new ValidationFailedException(
                new List<ValidationFailure>()
                {
                    new("Prop1 Name", "Prop1 error message"),
                    new("Prop2 Name", "Prop2 error message")
                });
        }
    }
}
