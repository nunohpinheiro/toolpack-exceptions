namespace ToolPack.Exceptions.Base.Entities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    ///   Exception related with an external component.
    /// </summary>
    [Serializable]
    public class ExternalComponentException : CustomBaseException
    {
        /// <summary>The default message.</summary>
        public new const string MessageDefault = "A failure related with an external component occurred.";

        /// <summary>Initializes a new instance of the ExternalComponentException class.</summary>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ExternalComponentException(Exception innerException = null)
            : base(MessageDefault, innerException) { }

        /// <summary>Initializes a new instance of the ExternalComponentException class.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ExternalComponentException(string message, Exception innerException = null)
            : base(SetValidMessageOrDefault(message, MessageDefault), innerException) { }

        /// <summary>Initializes a new instance of the ExternalComponentException class.</summary>
        /// <param name="componentName">Name of the component.</param>
        /// <param name="componentDescription">The component description.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ExternalComponentException(
            string componentName,
            string componentDescription,
            Exception innerException = null)
            : base(
                MessageDefault + $" | Component name: '{componentName}'. Component description: '{componentDescription}'.",
                innerException)
        { }

        /// <summary>Initializes a new instance of the <see cref="ExternalComponentException" /> class.</summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo">SerializationInfo</see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext">StreamingContext</see> that contains contextual information about the source or destination.</param>
        protected ExternalComponentException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
