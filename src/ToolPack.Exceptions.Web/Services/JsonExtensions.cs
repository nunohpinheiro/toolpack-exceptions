namespace ToolPack.Exceptions.Web.Services;

using System;
using System.Text.Json;

internal static class JsonExtensions
{
    /// <summary>Tries to serialize an object to JSON, using camel case.</summary>
    /// <typeparam name="T">The type of the input object.</typeparam>
    /// <param name="inputObject">The input object to be serialized.</param>
    /// <param name="outputJson">The output JSON string representation of the object. If the serialization fails, it will be null.</param>
    /// <param name="exception">The occurring exception, when the serialization fails. Otherwise, it will be null.</param>
    /// <returns>True, if the serialization succeeds; otherwise, false.</returns>
    internal static bool TrySerializeCamelCase<T>(this T inputObject, out string outputJson, out Exception exception)
    {
        outputJson = null;
        exception = null;
        try
        {
            outputJson = JsonSerializer.Serialize(inputObject, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return true;
        }
        catch (Exception ex)
        {
            exception = ex;
            return false;
        }
    }
}
