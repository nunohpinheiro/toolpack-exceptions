namespace ToolPack.Exceptions.UnitTests.Web.Extensions
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using ToolPack.Exceptions.Web.Extensions;

    public class JsonExtensionsTests
    {
        [Test]
        public void TrySerializeCamelCase_InputSerializable_ReturnsTrue_OutputStringMatches_OutputExceptionIsNotNull()
        {
            // Arrange
            SomeClass input = new() { SomeProperty = 1 };

            string expectedJson = "{\"someProperty\":1}";

            // Act
            var result = input.TrySerializeCamelCase(out string outputString, out Exception outputException);

            // Assert
            result.Should().BeTrue();
            outputString.Should().Be(expectedJson);
            outputException.Should().BeNull();
        }

        private class SomeClass
        {
            public int SomeProperty { get; set; }
        }
    }
}
