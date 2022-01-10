namespace ToolPack.Exceptions.Web.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using ToolPack.Exceptions.Web.Handlers;
    using ToolPack.Exceptions.Web.Services.Implementations;
    using ToolPack.Exceptions.Web.Services.Interfaces;

    /// <summary>Class with extension methods to inject dependencies regarding the ToolPack Exceptions framework.</summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Adds the ToolPack Exceptions framework services that treat exceptions.
        /// This does not include gRPC interceptors nor ASP.NET middleware (for such, see the other extension methods).</summary>
        /// <param name="services">The services.</param>
        /// <returns>The services updated with a registered Exceptions system.</returns>
        public static IServiceCollection AddToolPackExceptions(this IServiceCollection services)
        {
            services.AddExceptionServices();

            return services;
        }

        /// <summary>
        /// Adds all the ToolPack Exceptions framework required for gRPC services, to handle exceptions globally in gRPC calls.
        /// If handling exceptions in REST endpoints is required, proper middleware must be used (see extension "UseToolPackExceptionsMiddleware").</summary>
        /// <param name="services">The services.</param>
        /// <returns>The services updated with a registered Exceptions system (including gRPC interceptor services).</returns>
        public static IServiceCollection AddToolPackExceptionsGrpc(this IServiceCollection services)
        {
            services.AddToolPackExceptions()
                    .AddToolPackExceptionsGrpcInterceptor();

            return services;
        }

        /// <summary>
        /// Adds the ToolPack Exceptions interceptor services, to handle exceptions globally in gRPC calls.
        /// It must not be used in REST APIs; in such cases, exceptions must be handled with proper middleware (see extension "UseToolPackExceptionsMiddleware").
        /// It does not add the ToolPack Exceptions framework services that treat exceptions; for such, use "AddToolPackExceptionsGrpc" or "AddToolPackExceptions".</summary>
        /// <param name="services">The services.</param>
        /// <returns>The gRPC builder updated with the registered interceptor services.</returns>
        public static IServiceCollection AddToolPackExceptionsGrpcInterceptor(this IServiceCollection services)
        {
            services.AddGrpc(options => options.Interceptors.Add<GlobalExceptionInterceptor>());

            return services;
        }

        /// <summary>
        /// Uses the ToolPack Exceptions middleware, to handle exceptions globally in the request pipeline.
        /// It must not be used in gRPC APIs; in such cases, exceptions must be handled with interceptors (see extensions "AddToolPackExceptionsGrpc" and "AddToolPackExceptionsGrpcInterceptor").
        /// It does not add the ToolPack Exceptions framework services that treat exceptions, so the extension "AddToolPackExceptions" must also be used.</summary>
        /// <param name="appBuilder">The application builder.</param>
        /// <returns>The application builder updated with the registered middleware.</returns>
        public static IApplicationBuilder UseToolPackExceptionsMiddleware(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseMiddleware<GlobalExceptionMiddleware>();

            return appBuilder;
        }

        private static IServiceCollection AddExceptionServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor()
                    .AddScoped<IProblemDetailsService, ProblemDetailsService>();

            return services;
        }
    }
}
