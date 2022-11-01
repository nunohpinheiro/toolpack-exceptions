namespace RestAPI.CustomExceptions;

using System;
using System.Runtime.Serialization;
using ToolPack.Exceptions.Base.Entities;

[Serializable]
public class EnhanceYourCalmException : CustomBaseException
{
    public EnhanceYourCalmException()
        : base("Message Exception to enhance your calm") { }

    protected EnhanceYourCalmException(
        SerializationInfo serializationInfo,
        StreamingContext streamingContext)
        : base(serializationInfo, streamingContext) { }
}
