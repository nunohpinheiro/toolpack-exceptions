namespace ToolPack.Exceptions.Base.Entities;

using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

/// <summary>
///   Exception related with a validation failure.
/// </summary>
[Serializable]
public class ValidationFailedException : CustomBaseException
{
    /// <summary>The default message.</summary>
    public new const string MessageDefault = "One or more validation failures occurred.";

    /// <summary>Gets the errors.</summary>
    /// <value>The errors.</value>
    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();

    /// <summary>Initializes a new instance of the ValidationFailedException class.</summary>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public ValidationFailedException(Exception innerException = null)
        : base(MessageDefault, innerException) { }

    /// <summary>Initializes a new instance of the ValidationFailedException class.</summary>
    /// <param name="failures">The failures that occurred. (ValidationFailure elements, from FluentValidation)</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public ValidationFailedException(IEnumerable<ValidationFailure> failures, Exception innerException = null)
        : this(innerException)
    {
        var failureGroups = failures?.GroupBy(e => e.PropertyName, e => e.ErrorMessage) ?? Enumerable.Empty<IGrouping<string, string>>();

        foreach (var failureGroup in failureGroups)
        {
            var propertyName = failureGroup.Key;
            var propertyFailures = failureGroup.ToArray();

            Errors.Add(propertyName, propertyFailures);
        }
    }

    /// <summary>Initializes a new instance of the <see cref="ValidationFailedException" /> class.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo">SerializationInfo</see> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext">StreamingContext</see> that contains contextual information about the source or destination.</param>
    protected ValidationFailedException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
