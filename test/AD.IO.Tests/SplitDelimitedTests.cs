using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AD.IO.Paths;
using JetBrains.Annotations;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace AD.IO.Tests
{
    [PublicAPI]
    public class SplitDelimitedExtensionsTests
    {
        public static IEnumerable<object[]> TestData0 =>
            new List<object[]>
            {
                new object[]
                {
                    Delimiter.Parenthetical,
                    "a,b,c,d,e,f,g",
                    new string[] { "a", "b", "c", "d", "e", "f", "g" }
                },
                new object[]
                {
                    Delimiter.Parenthetical,
                    "a,b,c,group_1(d,e,f),g",
                    new string[] { "a", "b", "c", "group_1(d,e,f)", "g" }
                },
                new object[]
                {
                    Delimiter.Parenthetical,
                    "a,b,c,group_1(d,e,f),group_2(g)",
                    new string[] { "a", "b", "c", "group_1(d,e,f)", "group_2(g)" }
                },
                new object[]
                {
                    Delimiter.Quote,
                    "a,b,c,d,e,\"f\"",
                    new string[] { "a", "b", "c", "d", "e", "\"f\"" }
                },
                new object[]
                {
                    Delimiter.Quote,
                    "a,b,c,d,e,\"f,g\"",
                    new string[] { "a", "b", "c", "d", "e", "\"f,g\"" }
                },
                new object[]
                {
                    Delimiter.Quote,
                    "a,b,c,d,e,\"f,g\",\r\na,b,c,d,e|,\"f|g\"",
                    new string[] { "a", "b", "c", "d", "e", "\"f,g\"", "\r\na", "b", "c", "d", "e|", "\"f|g\"" }
                },
                new object[]
                {
                    Delimiter.Pipe,
                    "a|b|c|d|e|\"f|g\"|\r\na|b|c|d|e,|\"f,g\"",
                    new string[] { "a", "b", "c", "d", "e", "\"f|g\"", "\r\na", "b", "c", "d", "e,", "\"f,g\"" }
                }
            };

        public static IEnumerable<object[]> TestData1 =>
            new List<object[]>
            {
                new object[]
                {
                    ',',
                    "a,b,c,d,e,f,g",
                    new string[] { "a", "b", "c", "d", "e", "f", "g" }
                },
                new object[]
                {
                    ',',
                    "a,b,c,d,e,\"f\"",
                    new string[] { "a", "b", "c", "d", "e", "f" }
                },
                new object[]
                {
                    ',',
                    "a,b,c,d,e,\"f,g\"",
                    new string[] { "a", "b", "c", "d", "e", "f,g" }
                },
                new object[]
                {
                    ',',
                    "a,b,c,d,e,\"f,g\",\r\na,b,c,d,e|,\"f|g\"",
                    new string[] { "a", "b", "c", "d", "e", "f,g", "a", "b", "c", "d", "e|", "f|g" }
                },
                new object[]
                {
                    '|',
                    "a|b|c|d|e|\"f|g\"|\r\na|b|c|d|e,|\"f,g\"",
                    new string[] { "a", "b", "c", "d", "e", "f|g", "a", "b", "c", "d", "e,", "f,g" }
                }
            };

        [Theory]
        [MemberData(nameof(TestData0))]
        public void Test0(Delimiter delimiter, string value, StringValues expected)
        {
            StringValues result = value.Split(delimiter).ToArray();

            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(TestData1))]
        public void Test1(char delimiter, string value, StringValues expected)
        {
            StringValues result = value.SplitDelimitedLine(delimiter).ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
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
            IEnumerable<XNode> nodes = path.ReadXml();
            IEnumerable<string> result = nodes.OfType<XElement>().Elements().Select(x => (string) x).OrderBy(x => x);

            // Assert
            Assert.True(result.SequenceEqual(expected));
        }

        [Fact]
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
            IEnumerable<XNode> nodes = path.ReadXml();
            IEnumerable<string> result = nodes.OfType<XElement>().Elements().Select(x => (string) x).OrderBy(x => x);

            // Assert
            Assert.True(result.SequenceEqual(expected));
        }
    }
}