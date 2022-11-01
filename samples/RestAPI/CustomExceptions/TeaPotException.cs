namespace RestAPI.CustomExceptions;

using System;
using System.Runtime.Serialization;
using ToolPack.Exceptions.Base.Entities;

[Serializable]
public class TeaPotException : CustomBaseException
{
    public TeaPotException()
        : base("Message Exception that I'm a teapot") { }

    protected TeaPotException(
        SerializationInfo serializationInfo,
        StreamingContext streamingContext)
        : base(serializationInfo, streamingContext) { }
}
