namespace ToolPack.Exceptions.Base.Guard
{
    using System;
    using System.Linq;
    using ToolPack.Exceptions.Base.Entities;

    /// <summary>
    /// Class with methods to guard arguments/results against unwanted conditions. 
    /// If the unwanted conditions occur, exceptions are thrown.
    /// </summary>
    // TODO: Add similar class, but returning result responses instead of throwing
    public static class ThrowWhen
    {
        /// <summary>Throws an exception, if a condition is not met.</summary>
        /// <typeparam name="T">Exception type.</typeparam>
        /// <param name="condition">Condition to validate.</param>
        /// <param name="exception">Exception to throw.</param>
        public static void ConditionFails<T>(bool condition, T exception)
            where T : Exception
        {
            if (!condition)
            {
                throw exception;
            }
        }

        /// <summary>Throws an ArgumentNullException, if the argument is null.</summary>
        /// <typeparam name="T">Argument type.</typeparam>
        /// <param name="argument">Argument to validate.</param>
        /// <param name="argumentDescription">Description of the argument.</param>
        /// <param name="exceptionMessage">Message of the exception, in case it is thrown.</param>
        public static void ArgumentNull<T>(T argument, string argumentDescription, string exceptionMessage = null)
        {
            ConditionFails(argument is not null, new ArgumentNullException(argumentDescription, exceptionMessage));
        }

        /// <summary>Throws an ArgumentNullException when an argument of the list is null.</summary>
        /// <param name="argumentDefinitions">Collection of argument definitions to validate. Each definition includes the argument's object and description/name.</param>
        public static void ArgumentNull(params (object, string)[] argumentDefinitions)
        {
            foreach (var argDefinition in argumentDefinitions ?? Enumerable.Empty<(object, string)>())
            {
                ArgumentNull(argDefinition.Item1, argDefinition.Item2);
            }
        }

        /// <summary>Throws an applicational NotFoundException when the provided argument is null.</summary>
        /// <param name="argument">String argument to validate.</param>
        /// <param name="argumentDescription">Description/Name of the argument.</param>
        /// <param name="argumentKey">Key/identifier of the argument.</param>
        public static void ArgumentNullThrowNotFound<T>(T argument, string argumentDescription, string argumentKey)
        {
            ConditionFails(argument is not null, new NotFoundException(argumentDescription, argumentKey));
        }

        /// <summary>Throws an ArgumentOutOfRangeException, if a condition is not met.</summary>
        /// <param name="condition">Condition to validate.</param>
        /// <param name="argumentDescription">Description of the argument.</param>
        public static void ArgumentOutOfRange(bool condition, string argumentDescription)
        {
            ConditionFails(condition, new ArgumentOutOfRangeException(argumentDescription));
        }

        /// <summary>Throws an ArgumentException, if the argument string is null, empty or white spaces.</summary>
        /// <param name="argument">String argument to validate.</param>
        /// <param name="argumentDescription">Description of the argument.</param>
        public static void ArgumentStringNullOrWhiteSpace(string argument, string argumentDescription)
        {
            ConditionFails(!string.IsNullOrWhiteSpace(argument), new ArgumentException($"String cannot be null, empty or white space. Argument description: {argumentDescription}"));
        }

        /// <summary>Throws an ArgumentException when an argument string of the list is null, empty or white spaces.</summary>
        /// <param name="argumentDefinitions">Collection of argument definitions to validate. Each definition includes the argument's string value and description/name.</param>
        public static void ArgumentStringNullOrWhiteSpace(params (string, string)[] argumentDefinitions)
        {
            foreach (var argDefinition in argumentDefinitions ?? Enumerable.Empty<(string, string)>())
            {
                ArgumentStringNullOrWhiteSpace(argDefinition.Item1, argDefinition.Item2);
            }
        }
    }
}
