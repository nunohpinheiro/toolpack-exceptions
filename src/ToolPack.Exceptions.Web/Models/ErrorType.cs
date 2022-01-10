namespace ToolPack.Exceptions.Web.Models
{
    /// <summary>
    /// Error type descriptions.
    /// (Currently, a link to the explanation of the HTTP status.)</summary>
    internal static class ErrorType
    {
        // TODO: Expose these publicly
        internal const string AlreadyExists = "https://httpstatuses.com/409";
        internal const string BadRequest = "https://httpstatuses.com/400";
        internal const string Forbidden = "https://httpstatuses.com/403";
        internal const string InternalServerError = "https://httpstatuses.com/500";
        internal const string NotFound = "https://httpstatuses.com/404";
        internal const string NotImplemented = "https://httpstatuses.com/501";
        internal const string ServiceUnavailable = "https://httpstatuses.com/503";
        internal const string Unauthorized = "https://httpstatuses.com/401";
    }
}
