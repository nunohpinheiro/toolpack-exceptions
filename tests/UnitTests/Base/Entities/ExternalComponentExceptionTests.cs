namespace ToolPack.Exceptions.UnitTests.Base.Entities;

using FluentAssertions;
using NUnit.Framework;
using System;
using ToolPack.Exceptions.Base.Entities;

public class ExternalComponentExceptionTests
{
    private const string _expectedMessageDefault = ExternalComponentException.MessageDefault;

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void Ctor_EmptyMessage_ReturnsExceptionWithDefaultMessage(string message)
    {
        // Arrange
        var innerException = new Exception("just a sample inner exception");

        // Act
        var actual = new ExternalComponentException(message, innerException);

        // Assert
        actual.Should().Match<ExternalComponentException>(x =>
                            x.Message.Equals(_expectedMessageDefault)
                            && x.InnerException.Equals(innerException));
    }

    [Test]
    public void Ctor_WithComponent_ReturnsExceptionWithMatchingMessage()
    {
        // Arrange
        var componentName = "sample name";
        var componentDescription = "sample key";
        var expectedMessage = _expectedMessageDefault + $" | Component name: '{componentName}'. Component description: '{componentDescription}'.";

        // Act
        var actual = new ExternalComponentException(componentName, componentDescription);

        // Assert
        actual.Should().Match<ExternalComponentException>(x =>
                            x.Message.Equals(expectedMessage)
                            && x.InnerException == null);
    }

    [Test]
    public void Ctor_NoParameter_ReturnsExceptionWithDefaults()
    {
        // Act
        var actual = new ExternalComponentException();

        // Assert
        actual.Should().Match<ExternalComponentException>(x => x.Message.Equals(_expectedMessageDefault));
    }

    [Test]
    public void Ctor_WithInnerException_ReturnsExceptionWithDefaultsAndInnerException()
    {
        // Arrange
        var innerException = new Exception("just a sample inner exception");

        // Act
        var actual = new ExternalComponentException(innerException);

        // Assert
        actual.Should().Match<ExternalComponentException>(x =>
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
        var actual = new ExternalComponentException(message, innerException);

        // Assert
        actual.Should().Match<ExternalComponentException>(x =>
                            x.Message.Equals(message)
                            && x.InnerException.Equals(innerException));
    }
}
