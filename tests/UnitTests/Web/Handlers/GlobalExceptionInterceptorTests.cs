namespace ToolPack.Exceptions.UnitTests.Web.Handlers
{
    using FluentAssertions;
    using Grpc.Core;
    using Microsoft.Extensions.Logging;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;
    using ToolPack.Exceptions.Web.Handlers;
    using ToolPack.Exceptions.Web.Models;
    using ToolPack.Exceptions.Web.Services.Interfaces;

    public class GlobalExceptionInterceptorTests
    {
        private static Mock<ServerCallContext> ServerCallContextMock => new();
        private static Mock<ILogger<GlobalExceptionInterceptor>> LoggerMock => new();
        private static Mock<IProblemDetailsService> ProblemDetailsSvcMock => new();

        [Test]
        public void ClientStreamingServerHandler_MethodNotThrowsException_ProblemDetailsServiceIsNotCalled()
        {
            // Arrange
            Mock<IAsyncStreamReader<object>> requestMock = new();

            Mock<ClientStreamingServerMethod<object, object>> continuationMethodMock = new();

            var problemDetailsSvcMock = ProblemDetailsSvcMock;

            var exceptionsInterceptor = new GlobalExceptionInterceptor(LoggerMock.Object, problemDetailsSvcMock.Object);

            WebErrorStatus errorStatus = It.IsAny<WebErrorStatus>();

            // Act
            Func<Task> act = async () =>
                await exceptionsInterceptor.ClientStreamingServerHandler(requestMock.Object, ServerCallContextMock.Object, continuationMethodMock.Object);

            // Assert
            act.Should().NotThrow();
            problemDetailsSvcMock.Verify(
                x => x.BuildProblemDetailsResponse(It.IsAny<Exception>()),
                Times.Never);
        }

        [Test]
        public void ClientStreamingServerHandler_MethodThrowsException_ProblemDetailsServiceIsCalled()
        {
            // Arrange
            Mock<IAsyncStreamReader<object>> requestMock = new();

            var exception = new Exception("sample message");
            Mock<ClientStreamingServerMethod<object, object>> continuationMethodMock = new();
            continuationMethodMock.Setup(m => m(requestMock.Object, ServerCallContextMock.Object)).ThrowsAsync(exception);

            var problemDetailsSvcMock = ProblemDetailsSvcMock;

            var exceptionsInterceptor = new GlobalExceptionInterceptor(LoggerMock.Object, problemDetailsSvcMock.Object);

            WebErrorStatus errorStatus = It.IsAny<WebErrorStatus>();

            // Act
            Func<Task> act = async () =>
                await exceptionsInterceptor.ClientStreamingServerHandler(requestMock.Object, ServerCallContextMock.Object, continuationMethodMock.Object);

            // Assert
            act.Should().ThrowAsync<RpcException>();
            problemDetailsSvcMock.Verify(
                x => x.BuildProblemDetailsResponse(It.IsAny<Exception>()),
                Times.Never);
        }

        [Test]
        public void DuplexStreamingServerHandler_MethodNotThrowsException_ProblemDetailsServiceIsNotCalled()
        {
            // Arrange
            Mock<IAsyncStreamReader<object>> requestMock = new();
            Mock<IServerStreamWriter<object>> responseMock = new();

            Mock<DuplexStreamingServerMethod<object, object>> continuationMethodMock = new();

            var problemDetailsSvcMock = ProblemDetailsSvcMock;

            var exceptionsInterceptor = new GlobalExceptionInterceptor(LoggerMock.Object, problemDetailsSvcMock.Object);

            WebErrorStatus errorStatus = It.IsAny<WebErrorStatus>();

            // Act
            Func<Task> act = async () =>
                await exceptionsInterceptor.DuplexStreamingServerHandler(requestMock.Object, responseMock.Object, ServerCallContextMock.Object, continuationMethodMock.Object);

            // Assert
            act.Should().NotThrow();
            problemDetailsSvcMock.Verify(
                x => x.BuildProblemDetailsResponse(It.IsAny<Exception>()),
                Times.Never);
        }

        [Test]
        public void DuplexStreamingServerHandler_MethodThrowsException_ProblemDetailsServiceIsCalled()
        {
            // Arrange
            Mock<IAsyncStreamReader<object>> requestMock = new();
            Mock<IServerStreamWriter<object>> responseMock = new();

            var exception = new Exception("sample message");
            Mock<DuplexStreamingServerMethod<object, object>> continuationMethodMock = new();
            continuationMethodMock.Setup(m => m(requestMock.Object, responseMock.Object, ServerCallContextMock.Object)).ThrowsAsync(exception);

            var problemDetailsSvcMock = ProblemDetailsSvcMock;

            var exceptionsInterceptor = new GlobalExceptionInterceptor(LoggerMock.Object, problemDetailsSvcMock.Object);

            WebErrorStatus errorStatus = It.IsAny<WebErrorStatus>();

            // Act
            Func<Task> act = async () =>
                await exceptionsInterceptor.DuplexStreamingServerHandler(requestMock.Object, responseMock.Object, ServerCallContextMock.Object, continuationMethodMock.Object);

            // Assert
            act.Should().ThrowAsync<RpcException>();
            problemDetailsSvcMock.Verify(
                x => x.BuildProblemDetailsResponse(It.IsAny<Exception>()),
                Times.Never);
        }

        [Test]
        public void ServerStreamingServerHandler_MethodNotThrowsException_ProblemDetailsServiceIsNotCalled()
        {
            // Arrange
            Mock<IAsyncStreamReader<object>> requestMock = new();
            Mock<IServerStreamWriter<object>> responseMock = new();

            Mock<ServerStreamingServerMethod<object, object>> continuationMethodMock = new();

            var problemDetailsSvcMock = ProblemDetailsSvcMock;

            var exceptionsInterceptor = new GlobalExceptionInterceptor(LoggerMock.Object, problemDetailsSvcMock.Object);

            WebErrorStatus errorStatus = It.IsAny<WebErrorStatus>();

            // Act
            Func<Task> act = async () =>
                await exceptionsInterceptor.ServerStreamingServerHandler(requestMock.Object, responseMock.Object, ServerCallContextMock.Object, continuationMethodMock.Object);

            // Assert
            act.Should().NotThrow();
            problemDetailsSvcMock.Verify(
                x => x.BuildProblemDetailsResponse(It.IsAny<Exception>()),
                Times.Never);
        }

        [Test]
        public void ServerStreamingServerHandler_MethodThrowsException_ProblemDetailsServiceIsCalled()
        {
            // Arrange
            Mock<IAsyncStreamReader<object>> requestMock = new();
            Mock<IServerStreamWriter<object>> responseMock = new();

            var exception = new Exception("sample message");
            Mock<ServerStreamingServerMethod<object, object>> continuationMethodMock = new();
            continuationMethodMock.Setup(m => m(requestMock.Object, responseMock.Object, ServerCallContextMock.Object)).ThrowsAsync(exception);

            var problemDetailsSvcMock = ProblemDetailsSvcMock;

            var exceptionsInterceptor = new GlobalExceptionInterceptor(LoggerMock.Object, problemDetailsSvcMock.Object);

            WebErrorStatus errorStatus = It.IsAny<WebErrorStatus>();

            // Act
            Func<Task> act = async () =>
                await exceptionsInterceptor.ServerStreamingServerHandler(requestMock.Object, responseMock.Object, ServerCallContextMock.Object, continuationMethodMock.Object);

            // Assert
            act.Should().ThrowAsync<RpcException>();
            problemDetailsSvcMock.Verify(
                x => x.BuildProblemDetailsResponse(It.IsAny<Exception>()),
                Times.Never);
        }

        [Test]
        public void UnaryServerHandler_MethodNotThrowsException_ProblemDetailsServiceIsNotCalled()
        {
            // Arrange
            Mock<IAsyncStreamReader<object>> requestMock = new();

            Mock<UnaryServerMethod<object, object>> continuationMethodMock = new();

            var problemDetailsSvcMock = ProblemDetailsSvcMock;

            var exceptionsInterceptor = new GlobalExceptionInterceptor(LoggerMock.Object, problemDetailsSvcMock.Object);

            WebErrorStatus errorStatus = It.IsAny<WebErrorStatus>();

            // Act
            Func<Task> act = async () =>
                await exceptionsInterceptor.UnaryServerHandler(requestMock.Object, ServerCallContextMock.Object, continuationMethodMock.Object);

            // Assert
            act.Should().NotThrow();
            problemDetailsSvcMock.Verify(
                x => x.BuildProblemDetailsResponse(It.IsAny<Exception>()),
                Times.Never);
        }

        [Test]
        public void UnaryServerHandler_MethodThrowsException_ProblemDetailsServiceIsCalled()
        {
            // Arrange
            Mock<IAsyncStreamReader<object>> requestMock = new();

            var exception = new Exception("sample message");
            Mock<UnaryServerMethod<object, object>> continuationMethodMock = new();
            continuationMethodMock.Setup(m => m(requestMock.Object, ServerCallContextMock.Object)).ThrowsAsync(exception);

            var problemDetailsSvcMock = ProblemDetailsSvcMock;

            var exceptionsInterceptor = new GlobalExceptionInterceptor(LoggerMock.Object, problemDetailsSvcMock.Object);

            WebErrorStatus errorStatus = It.IsAny<WebErrorStatus>();

            // Act
            Func<Task> act = async () =>
                await exceptionsInterceptor.UnaryServerHandler(requestMock.Object, ServerCallContextMock.Object, continuationMethodMock.Object);

            // Assert
            act.Should().ThrowAsync<RpcException>();
            problemDetailsSvcMock.Verify(
                x => x.BuildProblemDetailsResponse(It.IsAny<Exception>()),
                Times.Never);
        }
    }
}
