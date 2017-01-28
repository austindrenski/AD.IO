using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AD.IO.Tests
{
    [TestClass]
    public class SplitDelimitedExtensionsTests
    {
        [TestMethod]
        public void SplitDelimitedCommaLineTest0()
        {
            // Arrange
            const string delimited = "a,b,c,d,e,\"f\"";

            // Act
            IEnumerable<string> result = delimited.SplitDelimitedLine(',');

            // Assert
            Assert.IsTrue(new string[] { "a", "b", "c", "d", "e", "f" }.SequenceEqual(result));
        }

        [TestMethod]
        public void SplitDelimitedCommaLineTest1()
        {
            // Arrange
            const string delimited = "a,b,c,d,e,\"f,g\"";

            // Act
            IEnumerable<string> result = delimited.SplitDelimitedLine(',');

            // Assert
            Assert.IsTrue(new string[] { "a", "b", "c", "d", "e", "f,g" }.SequenceEqual(result));
        }

        [TestMethod]
        public void SplitDelimitedCommaLineTest2()
        {
            // Arrange
            const string delimited = "a,b,c,d,e,\"f,g\",\r\na,b,c,d,e|,\"f|g\"";

            // Act
            IEnumerable<string> result = delimited.SplitDelimitedLine(',');

            // Assert
            Assert.IsTrue(new string[] { "a", "b", "c", "d", "e", "f,g", "a", "b", "c", "d", "e|", "f|g" }.SequenceEqual(result));
        }

        [TestMethod]
        public void SplitDelimitedCommaLineTest3()
        {
            // Arrange
            const string delimited = "a|b|c|d|e|\"f|g\"|\r\na|b|c|d|e,|\"f,g\"";

            // Act
            IEnumerable<string> result = delimited.SplitDelimitedLine('|');

            // Assert
            Assert.IsTrue(new string[] { "a", "b", "c", "d", "e", "f|g", "a", "b", "c", "d", "e,", "f,g" }.SequenceEqual(result));
        }

        [TestMethod]
        public void SplitDelimitedPipeLineTest0()
        {
            // Arrange
            const string delimited = "a|b|c|d|e|\"f\"";

            // Act
            IEnumerable<string> result = delimited.SplitDelimitedLine('|');

            // Assert
            Assert.IsTrue(new string[] { "a", "b", "c", "d", "e", "f" }.SequenceEqual(result));
        }

        [TestMethod]
        public void SplitDelimitedPipeLineTest1()
        {
            // Arrange
            const string delimited = "a|b|c|d|e|\"f,g\"";

            // Act
            IEnumerable<string> result = delimited.SplitDelimitedLine('|');

            // Assert
            Assert.IsTrue(new string[] { "a", "b", "c", "d", "e", "f,g" }.SequenceEqual(result));
        }

        [TestMethod]
        public void SplitDelimitedCommaLinesTest0()
        {
            // Arrange
            string[] delimited = new string[] { "a,b,c,d,e,\"f\"", "g,h,i,j,k,\"l\"" };
            string[][] expected = new string[][]
            {
                new string[] { "a", "b", "c", "d", "e", "f" },
                new string[] { "g", "h", "i", "j", "k", "l" }
            };

            // Act
            IEnumerable<IEnumerable<string>> strings = delimited.SplitDelimitedLine(',');
            string[][] result = strings as string[][] ?? strings.Select(x => x.ToArray()).ToArray();

            // Assert
            Assert.IsTrue(expected[0].SequenceEqual(result[0]) || expected[0].SequenceEqual(result[1]));
            Assert.IsTrue(expected[1].SequenceEqual(result[0]) || expected[1].SequenceEqual(result[1]));
        }

        [TestMethod]
        public void SplitDelimitedCommaLinesTest1()
        {
            // Arrange
            string[] delimited = new string[] { "a,b,c,d,e,\"f,f\"", "g,h,i,j,k,\"l,l\"" };
            string[][] expected = new string[][]
            {
                new string[] { "a", "b", "c", "d", "e", "f,f" },
                new string[] { "g", "h", "i", "j", "k", "l,l" }
            };

            // Act
            string[][] result = delimited.SplitDelimitedLine(',').Select(x => x.ToArray()).ToArray();

            // Assert
            Assert.IsTrue(expected[0].SequenceEqual(result[0]) || expected[0].SequenceEqual(result[1]));
            Assert.IsTrue(expected[1].SequenceEqual(result[0]) || expected[1].SequenceEqual(result[1]));
        }

        [TestMethod]
        public void SplitDelimitedCommaLinesTest2()
        {
            // Arrange
            string[] delimited = new string[] { "a,b,c,d,e,\"f\"", "g,h,i,j,k,\"l\"" };
            string[][] expected = new string[][]
            {
                new string[] { "a", "b", "c", "d", "e", "f" },
                new string[] { "g", "h", "i", "j", "k", "l" }
            };

            // Act
            IEnumerable<IEnumerable<string>> strings = delimited.AsParallel().SplitDelimitedLine(',');
            string[][] result = strings.Select(x => x.ToArray()).ToArray();

            // Assert
            Assert.IsTrue(expected[0].SequenceEqual(result[0]) || expected[0].SequenceEqual(result[1]));
            Assert.IsTrue(expected[1].SequenceEqual(result[0]) || expected[1].SequenceEqual(result[1]));
        }

        [TestMethod]
        public void SplitDelimitedCommaLinesTest3()
        {
            // Arrange
            string[] delimited = new string[] { "a,b,c,d,e,\"f,f\"", "g,h,i,j,k,\"l,l\"" };
            string[][] expected = new string[][]
            {
                new string[] { "a", "b", "c", "d", "e", "f,f" },
                new string[] { "g", "h", "i", "j", "k", "l,l" }
            };

            // Act
            string[][] result = delimited.AsParallel().SplitDelimitedLine(',').Select(x => x.ToArray()).ToArray();

            // Assert
            Assert.IsTrue(expected[0].SequenceEqual(result[0]) || expected[0].SequenceEqual(result[1]));
            Assert.IsTrue(expected[1].SequenceEqual(result[0]) || expected[1].SequenceEqual(result[1]));
        }

        [TestMethod]
        public void SplitDelimitedPipeLinesTest0()
        {
            // Arrange
            string[] delimited = new string[] { "a|b|c|d|e|\"f\"", "g|h|i|j|k|\"l\"" };
            string[][] expected = new string[][]
            {
                new string[] { "a", "b", "c", "d", "e", "f" },
                new string[] { "g", "h", "i", "j", "k", "l" }
            };

            // Act
            string[][] result = delimited.SplitDelimitedLine('|').Select(x => x.ToArray()).ToArray();

            // Assert
            Assert.IsTrue(expected[0].SequenceEqual(result[0]) || expected[0].SequenceEqual(result[1]));
            Assert.IsTrue(expected[1].SequenceEqual(result[0]) || expected[1].SequenceEqual(result[1]));
        }

        [TestMethod]
        public void SplitDelimitedPipeLinesTest1()
        {
            // Arrange
            string[] delimited = new string[] { "a|b|c|d|e|\"f,f\"", "g|h|i|j|k|\"l,l\"" };
            string[][] expected = new string[][]
            {
                new string[] { "a", "b", "c", "d", "e", "f,f" },
                new string[] { "g", "h", "i", "j", "k", "l,l" }
            };

            // Act
            string[][] result = delimited.SplitDelimitedLine('|').Select(x => x.ToArray()).ToArray();

            // Assert
            Assert.IsTrue(expected[0].SequenceEqual(result[0]) || expected[0].SequenceEqual(result[1]));
            Assert.IsTrue(expected[1].SequenceEqual(result[0]) || expected[1].SequenceEqual(result[1]));
        }

        [TestMethod]
        public void SplitDelimitedCommaFileToXDocumentTest0()
        {
            // Arrange
            DelimitedFilePath path = DelimitedFilePath.Create(Path.ChangeExtension(Path.GetTempFileName(), ".csv"), ',');
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("a,b,c,d,e,f");
                writer.WriteLine("a,b,c,d,e,\"f,f\"");
                writer.WriteLine("g,h,i,j,k,\"l,l\"");
            }
            IEnumerable<string> expected = new string[] { "a", "b", "c", "d", "e", "f,f", "g", "h", "i", "j", "k", "l,l" }.OrderBy(x => x);

            // Act
            IEnumerable<XElement> elements = path.ReadAsXml();
            IEnumerable<string> result = elements.Elements().Select(x => x?.Value).OrderBy(x => x);

            // Assert
            Assert.IsTrue(result.SequenceEqual(expected));
        }

        [TestMethod]
        public void SplitDelimitedPipeFileToXDocumentTest0()
        {
            // Arrange
            DelimitedFilePath path = DelimitedFilePath.Create(Path.ChangeExtension(Path.GetTempFileName(), ".csv"), '|');
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("a|b|c|d|e|f");
                writer.WriteLine("a|b|c|d|e|\"f,f\"");
                writer.WriteLine("g|h|i|j|k|\"l,l\"");
            }
            IEnumerable<string> expected = new string[] { "a", "b", "c", "d", "e", "f,f", "g", "h", "i", "j", "k", "l,l" }.OrderBy(x => x);

            // Act
            IEnumerable<XElement> elements = path.ReadAsXml();
            IEnumerable<string> result = elements.Elements().Select(x => x?.Value).OrderBy(x => x);

            // Assert
            Assert.IsTrue(result.SequenceEqual(expected));
        }
    }
}