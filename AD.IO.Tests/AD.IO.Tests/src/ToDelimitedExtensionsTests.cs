using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AD.IO.Tests
{
    [TestClass]
    public class ToDelimitedExtensionsTests
    {
        [TestMethod]
        public void ToDelimitedTest0()
        {
            // Arrange
            IEnumerable<string> test = new string[] {"a","b","c"};

            // Act
            string result = test.ToDelimited();

            // Assert
            Assert.AreEqual(result, "a|b|c");
        }

        [TestMethod]
        public void ToDelimitedTest1()
        {
            // Arrange
            IEnumerable<XElement> test = new XElement[]
            {
                new XElement("record", new XElement("a", "A"), new XElement("b", "B"), new XElement("c", "C"), new XElement("HTS10", "0123456789"), new XElement("z"), new XElement("zz", "0")),
                new XElement("record", new XElement("a", "D"), new XElement("b", "E"), new XElement("c", "F"), new XElement("HTS10", "0123456789"), new XElement("z"), new XElement("zz", "0")),
                new XElement("record", new XElement("a", "G"), new XElement("b", "H"), new XElement("c", "I"), new XElement("HTS10", "0123456789"), new XElement("z"), new XElement("zz", "0"))
            };

            // Act
            string result = test.ToDelimited();

            // Assert
            Assert.AreEqual(result, "A|B|C|0123456789||0\r\nD|E|F|0123456789||0\r\nG|H|I|0123456789||0");
        }

        [TestMethod]
        public void ToDelimitedTest2()
        {
            // Arrange
            IEnumerable<int> test = new int[] { 0, 1, 2 };

            // Act
            string result = test.ToDelimited(); 

            // Assert
            Assert.AreEqual(result, "0|1|2");
        }

        [TestMethod]
        public void ToDelimitedTest3()
        {
            // Arrange
            IEnumerable<XElement> enumerable = new XElement[]
            {
                new XElement("record", new XElement("a", "A"), new XElement("b", "B"), new XElement("c", "C"), new XElement("HTS10", "0123456789"), new XElement("z"), new XElement("zz", "0")),
                new XElement("record", new XElement("a", "D"), new XElement("b", "E"), new XElement("c", "F"), new XElement("HTS10", "0123456789"), new XElement("z"), new XElement("zz", "0")),
                new XElement("record", new XElement("a", "G"), new XElement("b", "H"), new XElement("c", "I"), new XElement("HTS10", "0123456789"), new XElement("z"), new XElement("zz", "0"))
            };
            XDocument document = new XDocument(new XElement("root", enumerable));

            // Act
            string result = document.ToDelimited();

            // Assert
            Assert.AreEqual(result, "a|b|c|HTS10|z|zz\r\nA|B|C|0123456789||0\r\nD|E|F|0123456789||0\r\nG|H|I|0123456789||0");
        }

        [TestMethod]
        public void ToDelimitedTest4()
        {
            // Arrange
            IEnumerable<XElement> enumerable = new XElement[]
            {
                new XElement("record", new XElement("a", "A"), new XElement("b", "B"), new XElement("c", "C"), new XElement("HTS10", "0123456789"), new XElement("z"), new XElement("zz", "0")),
                new XElement("record", new XElement("a", "D"), new XElement("b", "E"), new XElement("c", "F"), new XElement("HTS10", "0123456789"), new XElement("z"), new XElement("zz", "0")),
                new XElement("record", new XElement("a", "G"), new XElement("b", "H"), new XElement("c", "I"), new XElement("HTS10", "0123456789"), new XElement("z"), new XElement("zz", "0"))
            };
            XDocument document = new XDocument(new XElement("root", enumerable));

            // Act
            string result = document.ToDelimited("&");

            // Assert
            Assert.AreEqual(result, "a&b&c&HTS10&z&zz\r\nA&B&C&0123456789&&0\r\nD&E&F&0123456789&&0\r\nG&H&I&0123456789&&0");
        }

        [TestMethod]
        public void ToDelimitedTest5()
        {
            // Arrange
            IEnumerable<IEnumerable<string>> test = new string[][]
            {
                new string[] { "a", "b", "c" },
                new string[] { "d", "e", "f" },
                new string[] { "g", "h", "i" }
            };

            // Act
            string result = test.ToDelimited();

            // Assert
            Assert.AreEqual(result, "a|b|c\r\nd|e|f\r\ng|h|i");
        }

        [TestMethod]
        public void ToDelimitedTest6()
        {
            // Arrange
            IEnumerable<IEnumerable<int>> test = new int[][]
            {
                new int[] { 0, 1, 2 },
                new int[] { 3, 4, 5 },
                new int[] { 6, 7, 8 }
            };

            // Act
            string result = test.ToDelimited();

            // Assert
            Assert.AreEqual(result, "0|1|2\r\n3|4|5\r\n6|7|8");
        }
    }
}