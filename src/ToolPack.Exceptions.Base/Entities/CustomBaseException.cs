namespace ToolPack.Exceptions.Base.Entities;

using System;
using System.Runtime.Serialization;

/// <summary>
///   Exception related with a failure in an owned system.
/// </summary>
[Serializable]
public class CustomBaseException : Exception
{
    /// <summary>The default message.</summary>
    public const string MessageDefault = "A failure related with an owned system occurred.";

    /// <summary>Initializes a new instance of the CustomBaseException class.</summary>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public CustomBaseException(Exception innerException = null)
        : base(MessageDefault, innerException) { }

    /// <summary>Initializes a new instance of the CustomBaseException class.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public CustomBaseException(string message, Exception innerException = null)
        : base(SetValidMessageOrDefault(message, MessageDefault), innerException) { }

    /// <summary>Initializes a new instance of the <see cref="CustomBaseException" /> class.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo">SerializationInfo</see> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext">StreamingContext</see> that contains contextual information about the source or destination.</param>
    protected CustomBaseException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    /// <summary>Sets a valid message or default.</summary>
    /// <param name="message">The message to set.</param>
    /// <param name="defaultMessage">The default message.</param>
    /// <returns>A string with the defined message.</returns>
    protected static string SetValidMessageOrDefault(string message, string defaultMessage)
    {
        return string.IsNullOrWhiteSpace(message) ? defaultMessage : message;
    }
}
