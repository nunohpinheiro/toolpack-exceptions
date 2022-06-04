namespace ToolPack.Exceptions.UnitTests.Base.Entities;

using FluentAssertions;
using NUnit.Framework;
using System;
using ToolPack.Exceptions.Base.Entities;

public class CustomBaseExceptionTests
{
    private const string _expectedMessageDefault = CustomBaseException.MessageDefault;

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void Ctor_EmptyMessage_ReturnsExceptionWithDefaultMessage(string message)
    {
        // Arrange
        var innerException = new Exception("just a sample inner exception");

        // Act
        var actual = new CustomBaseException(message, innerException);

        // Assert
        actual.Should().Match<CustomBaseException>(x =>
                            x.Message.Equals(_expectedMessageDefault)
                            && x.InnerException.Equals(innerException));
    }

    [Test]
    public void Ctor_NoParameter_ReturnsExceptionWithDefaults()
    {
        // Act
        var actual = new CustomBaseException();

        // Assert
        actual.Should().Match<CustomBaseException>(x => x.Message.Equals(_expectedMessageDefault));
    }

    [Test]
    public void Ctor_WithInnerException_ReturnsExceptionWithDefaultsAndInnerException()
    {
        // Arrange
        var innerException = new Exception("just a sample inner exception");

        // Act
        var actual = new CustomBaseException(innerException);

        // Assert
        actual.Should().Match<CustomBaseException>(x =>
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
        var actual = new CustomBaseException(message, innerException);

        // Assert
        actual.Should().Match<CustomBaseException>(x =>
                            x.Message.Equals(message)
                            && x.InnerException.Equals(innerException));
    }
}
