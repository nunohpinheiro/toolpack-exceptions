namespace ToolPack.Exceptions.Web.Models
{
    using System;

    internal class ProblemDetailsWebResponse
    {
        internal ProblemDetails ProblemDetails { get; init; }
        internal WebErrorStatus WebErrorStatus { get; init; }

        /// <summary>Creates a ProblemDetailsWebResponse instance with properties derived from a given exception.</summary>
        /// <param name="exception">Exception from which the ProblemDetails and WebErrorStatus properties are constructed.</param>
        /// <param name="traceId">Tracing identifier related with the context of the ProblemDetails property.</param>
        internal ProblemDetailsWebResponse(Exception exception, string traceId)
        {
            ProblemDetails = new(exception, traceId);
            WebErrorStatus = WebErrorStatuses.GetFromException(exception);
        }
    }
}
