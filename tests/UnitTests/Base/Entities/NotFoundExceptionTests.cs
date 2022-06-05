namespace ToolPack.Exceptions.UnitTests.Base.Entities;

using FluentAssertions;
using NUnit.Framework;
using System;
using ToolPack.Exceptions.Base.Entities;

public class NotFoundExceptionTests
{
    private const string _expectedMessageDefault = NotFoundException.MessageDefault;

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void Ctor_EmptyMessage_ReturnsExceptionWithDefaultMessage(string message)
    {
        // Arrange
        var innerException = new Exception("just a sample inner exception");

        // Act
        var actual = new NotFoundException(message, innerException);

        // Assert
        actual.Should().Match<NotFoundException>(x =>
                            x.Message.Equals(_expectedMessageDefault)
                            && x.InnerException.Equals(innerException));
    }

    [Test]
    public void Ctor_WithEntity_ReturnsExceptionWithMatchingMessage()
    {
        // Arrange
        var entityName = "sample name";
        var entityKey = "sample key";
        var expectedMessage = _expectedMessageDefault + $" | Entity: '{entityName}'. Entity key: '{entityKey}'.";

        // Act
        var actual = new NotFoundException(entityName, entityKey);

        // Assert
        actual.Should().Match<NotFoundException>(x =>
                            x.Message.Equals(expectedMessage)
                            && x.InnerException == null);
    }

    [Test]
    public void Ctor_NoParameter_ReturnsExceptionWithDefaults()
    {
        // Act
        var actual = new NotFoundException();

        // Assert
        actual.Should().Match<NotFoundException>(x => x.Message.Equals(_expectedMessageDefault));
    }

    [Test]
    public void Ctor_WithInnerException_ReturnsExceptionWithDefaultsAndInnerException()
    {
        // Arrange
        var innerException = new Exception("just a sample inner exception");

        // Act
        var actual = new NotFoundException(innerException);

        // Assert
        actual.Should().Match<NotFoundException>(x =>
                            x.Message.Equals(_expectedMessageDefault)
                            && x.InnerException.Equals(innerException));
    }

    [Test]
    public void Ctor_WithMessageAndInnerException_ReturnsExceptionWithMessageAndInnerException()
    {
        // Arrange
        var message = "just a sample message";
        var innerException = new Exception("just a sample inner exception");

        // Act
        var actual = new NotFoundException(message, innerException);

        // Assert
        actual.Should().Match<NotFoundException>(x =>
                            x.Message.Equals(message)
                            && x.InnerException.Equals(innerException));
    }
}
