namespace ToolPack.Exceptions.Base.Entities;

using System;
using System.Runtime.Serialization;

/// <summary>
///   Exception related with an entity that already exists.
/// </summary>
[Serializable]
public class AlreadyExistsException : CustomBaseException
{
    /// <summary>The default message.</summary>
    public new const string MessageDefault = "A failure occurred due to an entity that already exists.";

    /// <summary>Initializes a new instance of the AlreadyExistsException class.</summary>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public AlreadyExistsException(Exception innerException = null)
        : base(MessageDefault, innerException) { }

    /// <summary>Initializes a new instance of the AlreadyExistsException class.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public AlreadyExistsException(string message, Exception innerException = null)
        : base(SetValidMessageOrDefault(message, MessageDefault), innerException) { }

    /// <summary>Initializes a new instance of the AlreadyExistsException class.</summary>
    /// <param name="entityName">Name of the entity.</param>
    /// <param name="entityKey">The entity key.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public AlreadyExistsException(
        string entityName,
        object entityKey,
        Exception innerException = null)
        : base(
              MessageDefault + $" | Entity: '{entityName}'. Entity key: '{entityKey}'.", innerException)
    { }

    /// <summary>Initializes a new instance of the <see cref="AlreadyExistsException" /> class.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo">SerializationInfo</see> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext">StreamingContext</see> that contains contextual information about the source or destination.</param>
    protected AlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
