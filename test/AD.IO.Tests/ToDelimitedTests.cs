using System;
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
            Assert.Equal($"a|b{Environment.NewLine}aa|bb{Environment.NewLine}aa|bb{Environment.NewLine}aa|bb{Environment.NewLine}", result);
        }

        [Fact]
        public void ToDelimitedTest0()
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
            string result = test.ToDelimited(false);

            // Assert
            Assert.Equal($"aa|bb{Environment.NewLine}aa|bb{Environment.NewLine}aa|bb{Environment.NewLine}", result);
        }

        [Fact]
        public void ToDelimitedTest1()
        {
            // Arrange
            IEnumerable<string> test = new string[] { "a", "b", "c" };

            // Act
            string result = test.ToDelimitedString();

            // Assert
            Assert.Equal("a|b|c", result);
        }

        [Fact]
        public void ToDelimitedTest2()
        {
            // Arrange
            IEnumerable<string> test = new string[] { "a", "b", "c" };

            // Act
            string result = test.ToDelimited(false);

            // Assert
            Assert.Equal($"a{Environment.NewLine}b{Environment.NewLine}c{Environment.NewLine}", result);
        }

        [Fact]
        public void ToDelimitedTest3()
        {
            // Arrange
            IEnumerable<XElement> test = new XElement[]
            {
                new XElement("record",
                    new XElement("a", "A"),
                    new XElement("b", "B"),
                    new XElement("c", "C"),
                    new XElement("HTS10", "0123456789"),
                    new XElement("z"),
                    new XElement("zz", "0")),
                new XElement("record",
                    new XElement("a", "D"),
                    new XElement("b", "E"),
                    new XElement("c", "F"),
                    new XElement("HTS10", "0123456789"),
                    new XElement("z"),
                    new XElement("zz", "0")),
                new XElement("record",
                    new XElement("a", "G"),
                    new XElement("b", "H"),
                    new XElement("c", "I"),
                    new XElement("HTS10", "0123456789"),
                    new XElement("z"),
                    new XElement("zz", "0"))
            };

            // Act
            string result = test.ToDelimited(false);

            // Assert
            Assert.Equal(
                $"A|B|C|0123456789||0{Environment.NewLine}D|E|F|0123456789||0{Environment.NewLine}G|H|I|0123456789||0{Environment.NewLine}",
                result);
        }

        [Fact]
        public void ToDelimitedTest4()
        {
            // Arrange
            IEnumerable<int> test = new int[] { 0, 1, 2 };

            // Act
            string result = test.ToDelimitedString();

            // Assert
            Assert.Equal("0|1|2", result);
        }

        [Fact]
        public void ToDelimitedTest5()
        {
            // Arrange
            IEnumerable<int> test = new int[] { 0, 1, 2 };

            // Act
            string result = test.ToDelimited(false);

            // Assert
            Assert.Equal($"0{Environment.NewLine}1{Environment.NewLine}2{Environment.NewLine}", result);
        }

        [Fact]
        public void ToDelimitedTest6()
        {
            // Arrange
            IEnumerable<IEnumerable<string>> test = new string[][]
            {
                new string[] { "a", "b", "c" },
                new string[] { "d", "e", "f" },
                new string[] { "g", "h", "i" }
            };

            // Act
            string result = test.Select(x => x.ToDelimitedString()).ToDelimited(false, '\n');

            // Assert
            Assert.Equal($"a|b|c{Environment.NewLine}d|e|f{Environment.NewLine}g|h|i{Environment.NewLine}", result);
        }

        [Fact]
        public void ToDelimitedTest7()
        {
            // Arrange
            IEnumerable<IEnumerable<int>> test = new int[][]
            {
                new int[] { 0, 1, 2 },
                new int[] { 3, 4, 5 },
                new int[] { 6, 7, 8 }
            };

            // Act
            string result = test.Select(x => x.ToDelimitedString()).ToDelimited(false, '\n');

            // Assert
            Assert.Equal($"0|1|2{Environment.NewLine}3|4|5{Environment.NewLine}6|7|8{Environment.NewLine}", result);
        }

        [Fact]
        public void ToDelimitedTest8()
        {
            // Arrange
            IEnumerable<string> test = new string[] { "a", "b|", "\"c|c\"", "d|\"e", null };

            // Act
            string result = test.ToDelimitedString();

            // Assert
            Assert.Equal("a|\"b|\"|\"\"\"c|c\"\"\"|\"d|\"\"e\"|", result);
        }

        [Fact]
        public void ToDelimitedTest9()
        {
            // Arrange
            XDocument document = new XDocument();

            // Act
            string test = document.ToDelimited();

            // Assert
            Assert.Equal(string.Empty, test);
        }

        [Fact]
        public void ToDelimitedTest10()
        {
            // Arrange
            XDocument document =
                new XDocument(
                    new XElement("root"));

            // Act
            string test = document.ToDelimited();

            // Assert
            Assert.Equal(string.Empty, test);
        }

        [Fact]
        public void ToDelimitedTest11()
        {
            // Arrange
            XDocument document =
                new XDocument(
                    new XElement("root",
                        new XElement("record",
                            new XElement("Field1", 1),
                            new XElement("Field2", 2),
                            new XElement("Field3", 3),
                            new XElement("Field4", 4),
                            new XElement("Field5", 5)),
                        new XElement("record",
                            new XElement("Field1", 2),
                            new XElement("Field2", 4),
                            new XElement("Field3", 6),
                            new XElement("Field4", 8),
                            new XElement("Field5", 10))));

            // Act
            string test = document.ToDelimited(',');

            // Assert
            Assert.Equal(
                $"Field1,Field2,Field3,Field4,Field5{Environment.NewLine}1,2,3,4,5{Environment.NewLine}2,4,6,8,10{Environment.NewLine}",
                test);
        }

        [Fact]
        public void AnonymousTypeToDelimited()
        {
            var items = new[]
            {
                new { a = "value_a1", b = "value_b1" },
                new { a = "value_a2", b = "value_b2" }
            };

            string value = items.ToDelimited(true, ',');

            Assert.Equal(
                $"a,b{Environment.NewLine}value_a1,value_b1{Environment.NewLine}value_a2,value_b2{Environment.NewLine}",
                value);
        }

        [Fact]
        public void AnonymousTypeToDelimited2()
        {
            var items =
                new[]
                {
                    new { a = 1, b = 2, c = 3 }
                };

            string value = items.ToDelimited(true, ',');

            Assert.Equal($"a,b,c{Environment.NewLine}1,2,3{Environment.NewLine}", value);
        }

    }
}