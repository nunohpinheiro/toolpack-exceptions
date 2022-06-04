namespace ToolPack.Exceptions.UnitTests.Base.Entities;

using FluentAssertions;
using NUnit.Framework;
using System;
using ToolPack.Exceptions.Base.Entities;

public class AlreadyExistsExceptionTests
{
    private const string _expectedMessageDefault = AlreadyExistsException.MessageDefault;

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void Ctor_EmptyMessage_ReturnsExceptionWithDefaultMessage(string message)
    {
        // Arrange
        var innerException = new Exception("just a sample inner exception");

        // Act
        var actual = new AlreadyExistsException(message, innerException);

        // Assert
        actual.Should().Match<AlreadyExistsException>(x =>
                            x.Message.Equals(_expectedMessageDefault)
                            && x.InnerException.Equals(innerException));
    }

    [Test]
    public void Ctor_NoParameter_ReturnsExceptionWithDefaults()
    {
        // Act
        var actual = new AlreadyExistsException();

        // Assert
        actual.Should().Match<AlreadyExistsException>(x => x.Message.Equals(_expectedMessageDefault));
    }

    [Test]
    public void Ctor_WithInnerException_ReturnsExceptionWithDefaultsAndInnerException()
    {
        // Arrange
        var innerException = new Exception("just a sample inner exception");

        // Act
        var actual = new AlreadyExistsException(innerException);

        // Assert
        actual.Should().Match<AlreadyExistsException>(x =>
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
        var actual = new AlreadyExistsException(message, innerException);

        // Assert
        actual.Should().Match<AlreadyExistsException>(x =>
                            x.Message.Equals(message)
                            && x.InnerException.Equals(innerException));
    }
}
