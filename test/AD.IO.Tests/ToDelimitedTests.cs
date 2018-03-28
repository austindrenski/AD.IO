using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace AD.IO.Tests
{
    public class ToDelimitedExtensionsTests
    {
        [Fact]
        public void ToDelimitedTest00()
        {
            // Arrange
            var test =
                new[]
                {
                    new { a = "aa", b = "bb" },
                    new { a = "aa", b = "bb" },
                    new { a = "aa", b = "bb" }
                };

            // Act
            string result = test.ToDelimited();

            // Assert
            Assert.Equal("aa|bb\r\naa|bb\r\naa|bb", result);
        }

        [Fact]
        public void ToDelimitedTest0()
        {
            // Arrange
            IEnumerable<string> test = new string[] { "a", "b", "c" };

            // Act
            string result = test.ToDelimited();

            // Assert
            Assert.Equal("a|b|c", result);
        }

        [Fact]
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
            Assert.Equal("A|B|C|0123456789||0\r\nD|E|F|0123456789||0\r\nG|H|I|0123456789||0", result);
        }

        [Fact]
        public void ToDelimitedTest2()
        {
            // Arrange
            IEnumerable<XElement> test = new XElement[]
            {
                new XElement("record", new XElement("a", "A"), new XElement("b", "B"), new XElement("c", "C"), new XElement("HTS10", "0123456789"), new XElement("z"), new XElement("zz", "0")),
                new XElement("record", new XElement("a", "D"), new XElement("b", "E"), new XElement("c", "F"), new XElement("HTS10", "0123456789"), new XElement("z"), new XElement("zz", "0")),
                new XElement("record", new XElement("a", "G"), new XElement("b", "H"), new XElement("c", "I"), new XElement("HTS10", "0123456789"), new XElement("z"), new XElement("zz", "0"))
            };

            // Act
            string result = test.AsParallel().ToDelimited() ?? "";

            // Assert
            Assert.True("A|B|C|0123456789||0\r\nD|E|F|0123456789||0\r\nG|H|I|0123456789||0".All(x => result.Contains(x)));
        }

        [Fact]
        public void ToDelimitedTest3()
        {
            // Arrange
            IEnumerable<int> test = new int[] { 0, 1, 2 };

            // Act
            string result = test.ToDelimited();

            // Assert
            Assert.Equal("0|1|2", result);
        }

        [Fact]
        public void ToDelimitedTest4()
        {
            // Arrange
            IEnumerable<int> test = new int[] { 0, 1, 2 };

            // Act
            string result = test.AsParallel().ToDelimited() ?? "";

            // Assert
            Assert.True("0|1|2".All(x => result.Contains(x)));
        }

        [Fact]
        public void ToDelimitedTest5()
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
            Assert.Equal("a|b|c|HTS10|z|zz\r\nA|B|C|0123456789||0\r\nD|E|F|0123456789||0\r\nG|H|I|0123456789||0", result);
        }

        [Fact]
        public void ToDelimitedTest6()
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
            Assert.Equal("a&b&c&HTS10&z&zz\r\nA&B&C&0123456789&&0\r\nD&E&F&0123456789&&0\r\nG&H&I&0123456789&&0", result);
        }

        [Fact]
        public void ToDelimitedTest7()
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
            Assert.Equal("a|b|c\r\nd|e|f\r\ng|h|i", result);
        }

        [Fact]
        public void ToDelimitedTest8()
        {
            // Arrange
            IEnumerable<IEnumerable<string>> test = new string[][]
            {
                new string[] { "a", "b", "c" },
                new string[] { "d", "e", "f" },
                new string[] { "g", "h", "i" }
            };

            // Act
            string result = test.AsParallel().ToDelimited() ?? "";

            // Assert
            Assert.True("a|b|c\r\nd|e|f\r\ng|h|i".All(x => result.Contains(x)));
        }

        [Fact]
        public void ToDelimitedTest9()
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
            Assert.Equal("0|1|2\r\n3|4|5\r\n6|7|8", result);
        }

        [Fact]
        public void ToDelimitedTest10()
        {
            // Arrange
            IEnumerable<IEnumerable<int>> test = new int[][]
            {
                new int[] { 0, 1, 2 },
                new int[] { 3, 4, 5 },
                new int[] { 6, 7, 8 }
            };

            // Act
            string result = test.AsParallel().ToDelimited() ?? "";

            // Assert
            Assert.True(result.All(x => "0|1|2\r\n3|4|5\r\n6|7|8".Contains(x)));
        }

        [Fact]
        public void ToDelimitedTest11()
        {
            // Arrange
            XDocument document = new XDocument();

            // Act
            string test = document.ToDelimited();

            // Assert
            Assert.Null(test);
        }

        [Fact]
        public void ToDelimitedTest12()
        {
            // Arrange
            XDocument document = new XDocument(new XElement("root"));

            // Act
            string test = document.ToDelimited();

            // Assert
            Assert.Null(test);
        }

        [Fact]
        public void ToDelimitedTest13()
        {
            // Arrange
            IEnumerable<string> test = new string[] { "a", "b|", "\"c|c\"", null };

            // Act
            string result = test.ToDelimited() ?? "";

            // Assert
            Assert.True("a|b|\"c|c\"|".All(x => result.Contains(x)));
        }

        [Fact]
        public void ToDelimitedTest14()
        {
            // Arrange
            IEnumerable<string> test = new string[] { "a", "b|", "\"c|c\"", null };

            // Act
            string result = test.AsParallel().ToDelimited() ?? "";

            // Assert
            Assert.True("a|b|\"c|c\"|".All(x => result.Contains(x)));
        }
    }
}