using AjdExtensions.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AjdExtensionsTests
{
    [TestClass]
    public class ToCapitalizedExtensionsTests
    {
        [TestMethod]
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