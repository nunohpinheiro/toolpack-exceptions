namespace ToolPack.Exceptions.UnitTests.Base.Entities
{
    using FluentAssertions;
    using FluentValidation.Results;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ToolPack.Exceptions.Base.Entities;

    public class ValidationFailedExceptionTests
    {
        private const string _expectedMessageDefault = ValidationFailedException.MessageDefault;
        private static IDictionary<string, string[]> ExpectedDefaultErrors { get; } = new Dictionary<string, string[]>();

        [Test]
        public void Ctor_NoParameter_ReturnsExceptionWithDefaults()
        {
            // Act
            var actual = new ValidationFailedException();

            // Assert
            actual.Should().Match<ValidationFailedException>(x =>
                                x.Message.Equals(_expectedMessageDefault)
                                && x.InnerException == null);
            actual.Errors.Should().BeEquivalentTo(ExpectedDefaultErrors);
        }

        [Test]
        public void Ctor_WithInnerException_ReturnsExceptionWithDefaultsAndInnerException()
        {
            // Arrange
            var innerException = new Exception("just a sample inner exception");

            // Act
            var actual = new ValidationFailedException(innerException);

            // Assert
            actual.Should().Match<ValidationFailedException>(x =>
                                x.Message.Equals(_expectedMessageDefault)
                                && x.InnerException.Equals(innerException));
            actual.Errors.Should().BeEquivalentTo(ExpectedDefaultErrors);
        }

        [Test]
        public void Ctor_WithInnerExceptionAndEmptyValidationFailures_MatchesGivenParametersAndDefaultMessage()
        {
            // Arrange
            IEnumerable<ValidationFailure> failures = Enumerable.Empty<ValidationFailure>();
            var innerException = new Exception("just a sample inner exception");

            // Act
            var actual = new ValidationFailedException(failures, innerException);

            // Assert
            actual.Should().Match<ValidationFailedException>(x =>
                                x.Message.Equals(_expectedMessageDefault)
                                && x.InnerException.Equals(innerException));
            actual.Errors.Should().BeEquivalentTo(ExpectedDefaultErrors);
        }

        [Test]
        public void Ctor_WithInnerExceptionAndValidationFailures_MatchesGivenParametersAndDefaultMessage()
        {
            // Arrange
            var innerException = new Exception("just a sample inner exception");

            List<ValidationFailure> failures = new()
            {
                new("prop1", "msg11"),
                new("prop1", "msg12"),
                new("prop2", "msg21")
            };
            Dictionary<string, string[]> expectedErrors = new()
            {
                { "prop1", new string[] { "msg11", "msg12" } },
                { "prop2", new string[] { "msg21" } }
            };

            // Act
            var actual = new ValidationFailedException(failures, innerException);

            // Assert
            actual.Should().Match<ValidationFailedException>(x =>
                                x.Message.Equals(_expectedMessageDefault)
                                && x.InnerException.Equals(innerException));
            actual.Errors.Should().BeEquivalentTo(expectedErrors);
        }
    }
}
