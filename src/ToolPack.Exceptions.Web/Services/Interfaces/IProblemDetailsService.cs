using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ToolPack.Exceptions.UnitTests")]
namespace ToolPack.Exceptions.Web.Services.Interfaces
{
    using System;
    using ToolPack.Exceptions.Web.Models;

    internal interface IProblemDetailsService
    {
        /// <summary>
        /// Builds a JSON formatted response with the Problem Details format (according to the RFC 7807 (https://tools.ietf.org/html/rfc7807),
        /// as well as the error status of the response.
        /// </summary>
        /// <param name="exception">The exception to turn into Problem Details and matching error codes.</param>
        /// <returns>A JSON string with the Problem Details response and its related error status codes.</returns>
        (string, WebErrorStatus) BuildProblemDetailsResponse(Exception exception);
    }
}
