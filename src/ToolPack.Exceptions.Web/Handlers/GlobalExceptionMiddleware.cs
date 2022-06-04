using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ToolPack.Exceptions.UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace ToolPack.Exceptions.Web.Handlers;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mime;
using System.Threading.Tasks;
using ToolPack.Exceptions.Web.Services.Interfaces;

/// <summary>
/// Middleware to handle exceptions in ASP.NET REST services globally.
/// </summary>
internal class GlobalExceptionMiddleware
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(
        ILogger<GlobalExceptionMiddleware> logger,
        RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IProblemDetailsService problemDetailsService)
    {
        _logger.LogInformation("Incoming request in GlobalExceptionMiddleware.");

        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception was caught by the GlobalExceptionMiddleware. Exception: {Exception}", ex);
            await HandleExceptionAsync(httpContext, problemDetailsService, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, IProblemDetailsService problemDetailsService, Exception ex)
    {
        var (problemDetailsResponse, errorStatus) = problemDetailsService.BuildProblemDetailsResponse(ex);

        _logger.LogInformation(
            "A ProblemDetails response was built with error status. ErrorStatus: {ErrorStatus} | ProblemDetails: {ProblemDetails}",
            errorStatus,
            problemDetailsResponse);

        if (httpContext?.Response is not null)
        {
            httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            httpContext.Response.StatusCode = (int)errorStatus.HttpCode;
            await httpContext.Response.WriteAsync(problemDetailsResponse);
        }
        return;
    }
}
