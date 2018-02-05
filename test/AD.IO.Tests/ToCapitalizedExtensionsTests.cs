using System.Linq;
using JetBrains.Annotations;

namespace AD.IO.Tests
{
    [TestClass]
    public class ToCapitalizedExtensionsTests
    {
        [Theory]
        public void ToCapitalizeFirstTest()
        {
            // Arrange
            const string test = "lowercase";
            const string expected = "Lowercase";

            // Act
            string result = test.ToCapitalizeFirst();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}