namespace ToolPack.Exceptions.Web.DependencyInjection;

using System;
using ToolPack.Exceptions.Web.Models;

/// <summary>Options to customize mappings of Exceptions with matching WebErrorStatus.</summary>
public class ExceptionToWebErrorOptions
{
    /// <summary>Maps the specified Exception type with a WebErrorStatus.</summary>
    /// <typeparam name="TException">The type of the exception to be mapped.</typeparam>
    /// <param name="webErrorStatus">The web error status to be mapped onto.</param>
    public ExceptionToWebErrorOptions Map<TException>(WebErrorStatus webErrorStatus)
        where TException : Exception
    {
        ExceptionToWebErrorMap.AddOrReplace<TException>(webErrorStatus);
        return this;
    }
}
