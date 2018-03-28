using Xunit;

namespace AD.IO.Tests
{
    public class ToCapitalizedExtensionsTests
    {
        [Fact]
        public void ToCapitalizeFirstTest()
        {
            // Arrange
            const string test = "lowercase";
            const string expected = "Lowercase";

            // Act
            string result = test.ToCapitalizeFirst();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}