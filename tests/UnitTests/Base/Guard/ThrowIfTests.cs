namespace ToolPack.Exceptions.UnitTests.Base.Guard;

using FluentAssertions;
using NUnit.Framework;
using System;
using ToolPack.Exceptions.Base.Entities;
using ToolPack.Exceptions.Base.Guard;

public class ThrowIfTests
{
    private const string StringNullOrWhiteSpaceMsg = "String cannot be null, empty or white space. Argument description: ";

    [Test]
    public void ArgumentNull_EmptyParams_NotThrowsException()
    {
        // Act
        Action act = () => ThrowIf.ArgumentNull();

        // Assert
        act.Should().NotThrow();
    }

    [Test]
    public void ArgumentNull_Params_ConditionsVerify_NotThrowsException()
    {
        // Arrange
        int? argument1 = 3;
        int? argument2 = 4;

        var paramsArray = new (object, string)[]
        {
            (argument1, nameof(argument1)),
            (argument2, nameof(argument2))
        };

        // Act
        Action act = () => ThrowIf.ArgumentNull(paramsArray);

        // Assert
        act.Should().NotThrow();
    }

    [Test]
    public void ArgumentNull_Params_FirstConditionNotVerifies_ThrowsArgumentNullException()
    {
        // Arrange
        int? argument1 = null;
        int? argument2 = 3;

        var paramsArray = new (object, string)[]
        {
            (argument1, nameof(argument1)),
            (argument2, nameof(argument2))
        };

        // Act
        Action act = () => ThrowIf.ArgumentNull(paramsArray);

        // Assert
        act.Should().Throw<ArgumentNullException>()
                    .Which.ParamName.Should().Be(nameof(argument1));
    }

    [Test]
    public void ArgumentNull_Params_SecondConditionNotVerifies_ThrowsArgumentNullException()
    {
        // Arrange
        int? argument1 = 3;
        int? argument2 = null;

        var paramsArray = new (object, string)[]
        {
            (argument1, nameof(argument1)),
            (argument2, nameof(argument2))
        };

        // Act
        Action act = () => ThrowIf.ArgumentNull(paramsArray);

        // Assert
        act.Should().Throw<ArgumentNullException>()
                    .Which.ParamName.Should().Be(nameof(argument2));
    }

    [Test]
    public void ArgumentNullT_ConditionNotVerifies_ThrowsArgumentNullException()
    {
        // Arrange
        int? argument = null;
        string argumentDescription = nameof(argument);

        // Act
        Action act = () => ThrowIf.ArgumentNull(argument);

        // Assert
        act.Should().Throw<ArgumentNullException>()
                    .Which.ParamName.Should().Be(argumentDescription);
    }

    [Test]
    public void ArgumentNullT_ConditionVerifies_NotThrowsException()
    {
        // Arrange
        int? argument = 3;

        // Act
        var result = ThrowIf.ArgumentNull(argument);

        // Assert
        result.Should().Be(argument);
    }

    [Test]
    public void ArgumentNullThrowNotFound_ConditionNotVerifies_ThrowsNotFoundException()
    {
        // Arrange
        int? argument = null;
        string argumentDescription = nameof(argument);
        string argumentKey = "argument key";

        NotFoundException expectedException = new(argumentDescription, argumentKey);

        // Act
        Action act = () => ThrowIf.ArgumentNullThrowNotFound(argument, argumentKey);

        // Assert
        act.Should().Throw<NotFoundException>()
                    .WithMessage(expectedException.Message);
    }

    [Test]
    public void ArgumentNullThrowNotFound_ConditionVerifies_NotThrowsException()
    {
        // Arrange
        int? argument = 3;

        // Act
        var result = ThrowIf.ArgumentNullThrowNotFound(argument, "argument key");

        // Assert
        result.Should().Be(argument);
    }

    [Test]
    public void ArgumentOutOfRange_ConditionNotVerifies_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        bool condition = false;
        string argumentDescription = "sample description";

        ArgumentOutOfRangeException expectedException = new(argumentDescription);

        // Act
        Action act = () => ThrowIf.ArgumentOutOfRange(condition, argumentDescription);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
                    .WithMessage(expectedException.Message);
    }

    [Test]
    public void ArgumentOutOfRange_ConditionVerifies_NotThrowsException()
    {
        // Arrange
        bool condition = true;
        string argumentDescription = "sample description";

        // Act
        Action act = () => ThrowIf.ArgumentOutOfRange(condition, argumentDescription);

        // Assert
        act.Should().NotThrow();
    }

    [Test]
    public void ArgumentStringNullOrWhiteSpace_EmptyParams_NotThrowsException()
    {
        // Act
        Action act = () => ThrowIf.ArgumentStringNullOrWhiteSpace();

        // Assert
        act.Should().NotThrow();
    }

    [Test]
    public void ArgumentStringNullOrWhiteSpace_Params_ConditionsVerify_NotThrowsException()
    {
        // Arrange
        string argument1 = "such a valid string";
        string argument2 = "such a valid string";

        var paramsArray = new (string, string)[]
        {
            (argument1, nameof(argument1)),
            (argument2, nameof(argument2))
        };

        // Act
        Action act = () => ThrowIf.ArgumentStringNullOrWhiteSpace(paramsArray);

        // Assert
        act.Should().NotThrow();
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void ArgumentStringNullOrWhiteSpace_Params_FirstConditionNotVerifies_ThrowsArgumentException(string invalidString)
    {
        // Arrange
        string argument1 = invalidString;
        string argument2 = "such a valid string";

        var paramsArray = new (string, string)[]
        {
            (argument1, nameof(argument1)),
            (argument2, nameof(argument2))
        };

        var expectedMessage = $"{StringNullOrWhiteSpaceMsg}{nameof(argument1)}";

        // Act
        Action act = () => ThrowIf.ArgumentStringNullOrWhiteSpace(paramsArray);

        // Assert
        act.Should().Throw<ArgumentException>()
                    .WithMessage(expectedMessage);
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void ArgumentStringNullOrWhiteSpace_Params_SecondConditionNotVerifies_ThrowsArgumentException(string invalidString)
    {
        // Arrange
        string argument1 = "such a valid string";
        string argument2 = invalidString;

        var paramsArray = new (string, string)[]
        {
            (argument1, nameof(argument1)),
            (argument2, nameof(argument2))
        };

        var expectedMessage = $"{StringNullOrWhiteSpaceMsg}{nameof(argument2)}";

        // Act
        Action act = () => ThrowIf.ArgumentStringNullOrWhiteSpace(paramsArray);

        // Assert
        act.Should().Throw<ArgumentException>()
                    .WithMessage(expectedMessage);
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void ArgumentStringNullOrWhiteSpace_ConditionNotVerifies_ThrowsArgumentException(string invalidString)
    {
        // Arrange
        string argument = invalidString;
        string argumentDescription = nameof(argument);

        var expectedMessage = $"{StringNullOrWhiteSpaceMsg}{argumentDescription}";

        // Act
        Action act = () => ThrowIf.ArgumentStringNullOrWhiteSpace(argument);

        // Assert
        act.Should().Throw<ArgumentException>()
                    .WithMessage(expectedMessage);
    }

    [Test]
    public void ArgumentStringNullOrWhiteSpace_ConditionVerifies_NotThrowsException()
    {
        // Arrange
        string argument = "such a valid string";

        // Act
        var result = ThrowIf.ArgumentStringNullOrWhiteSpace(argument);

        // Assert
        result.Should().Be(argument);
    }

    [Test]
    public void ConditionFails_ConditionNotVerifies_ThrowsException()
    {
        // Arrange
        var condition = false;
        var exception = new Exception("sample exception");

        // Act
        Action act = () => ThrowIf.ConditionFails(condition, exception);

        // Assert
        act.Should().Throw<Exception>()
                    .WithMessage(exception.Message);
    }

    [Test]
    public void ConditionFails_ConditionVerifies_NotThrowsException()
    {
        // Arrange
        var condition = true;
        var exception = new Exception("sample exception");

        // Act
        Action act = () => ThrowIf.ConditionFails(condition, exception);

        // Assert
        act.Should().NotThrow();
    }
}
