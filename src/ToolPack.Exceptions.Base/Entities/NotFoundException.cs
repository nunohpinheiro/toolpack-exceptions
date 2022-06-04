namespace ToolPack.Exceptions.Base.Entities;

using System;
using System.Runtime.Serialization;

/// <summary>
///   Exception related with an entity that was not found.
/// </summary>
[Serializable]
public class NotFoundException : CustomBaseException
{
    /// <summary>The default message.</summary>
    public new const string MessageDefault = "A failure occurred due to an entity that was not found.";

    /// <summary>Initializes a new instance of the NotFoundException class.</summary>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public NotFoundException(Exception innerException = null)
        : base(MessageDefault, innerException) { }

    /// <summary>Initializes a new instance of the NotFoundException class.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public NotFoundException(string message, Exception innerException = null)
        : base(SetValidMessageOrDefault(message, MessageDefault), innerException) { }

    /// <summary>Initializes a new instance of the NotFoundException class.</summary>
    /// <param name="entityName">Name of the entity.</param>
    /// <param name="entityKey">The entity key.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public NotFoundException(
        string entityName,
        string entityKey,
        Exception innerException = null)
        : base(
              MessageDefault + $" | Entity: '{entityName}'. Entity key: '{entityKey}'.", innerException)
    { }

    /// <summary>Initializes a new instance of the <see cref="NotFoundException" /> class.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo">SerializationInfo</see> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext">StreamingContext</see> that contains contextual information about the source or destination.</param>
    protected NotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
