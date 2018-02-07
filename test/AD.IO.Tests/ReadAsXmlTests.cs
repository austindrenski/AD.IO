using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AD.IO.Paths;
using Xunit;

namespace AD.IO.Tests
{
    public class ReadAsXmlTests
    {
        [Fact]
        public void ReadAsXmlTest0()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "test_read_as_xml.csv");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            DelimitedFilePath delimitedPath = DelimitedFilePath.Create(path, ',');

            // Act
            IEnumerable<XElement> test = delimitedPath.ReadAsXml();

            // Assert
            Assert.Empty(test);
        }

        [Fact]
        public void ReadAsXmlTest1()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "test_read_as_xml.docx");

            DocxFilePath docx = DocxFilePath.Create(path, true);

            // Act
            XElement element = docx.ReadAsXml();

            // Assert
            Assert.Equal(docx.Name, (string) element?.Attribute("fileName"));
        }

        [Fact]
        public void ReadAsXmlTest2()
        {
            // Arrange
            string path0 = Path.Combine(Path.GetTempPath(), "test_read_as_xml.docx");
            string path1 = Path.Combine(Path.GetTempPath(), "test2.docx");

            DocxFilePath docx0 = DocxFilePath.Create(path0, true);
            DocxFilePath docx1 = DocxFilePath.Create(path1, true);

            DocxFilePath[] files = new DocxFilePath[] { docx0, docx1 };

            // Act
            IEnumerable<XElement> elements = files.ReadAsXml();

            // Assert
            Assert.True(elements.All(x => x != null));
        }

        [Fact]
        public void ReadAsXmlTest3()
        {
            // Arrange
            string path0 = Path.Combine(Path.GetTempPath(), "test_read_as_xml.docx");
            string path1 = Path.Combine(Path.GetTempPath(), "test2.docx");

            DocxFilePath docx0 = DocxFilePath.Create(path0, true);
            DocxFilePath docx1 = DocxFilePath.Create(path1, true);

            DocxFilePath[] files = new DocxFilePath[] { docx0, docx1 };

            // Act
            IEnumerable<XElement> elements = files.AsParallel().ReadAsXml();

            // Assert
            Assert.True(elements.All(x => x != null));
        }
    }
}