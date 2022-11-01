namespace ToolPack.Exceptions.Web.Services.Interfaces;

using System;
using ToolPack.Exceptions.Web.Models;

internal interface IProblemDetailsService
{
    /// <summary>
    /// Builds a JSON-formatted response with Problem Details (according to the RFC 7807 - https://tools.ietf.org/html/rfc7807),
    /// along with the matching error status of the response.
    /// </summary>
    /// <param name="exception">The exception to turn into Problem Details and matching error codes.</param>
    /// <returns>A JSON string with the Problem Details response and its related error status codes.</returns>
    (string, WebErrorStatus) BuildProblemDetailsResponse<T>(T exception) where T : Exception;
}
