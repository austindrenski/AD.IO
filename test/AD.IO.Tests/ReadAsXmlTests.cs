﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace AD.IO.Tests
{
    [TestClass]
    public class ReadAsXmlTests
    {
        [Theory]
        [ExpectedException(typeof(ArgumentException))]
        public void ReadAsXmlTest0()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "test.csv");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            DelimitedFilePath delimitedPath = DelimitedFilePath.Create(path, ',');

            // Act
            // ReSharper disable once UnusedVariable
            IEnumerable<XElement> element = delimitedPath.ReadAsXml();
        }

        [Theory]
        public void ReadAsXmlTest1()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "test.docx");
            DocxFilePath docx = DocxFilePath.Create(path);

            // Act
            XElement element = docx.ReadAsXml();

            // Assert
            Assert.IsTrue(element != null);
        }

        [Theory]
        public void ReadAsXmlTest2()
        {
            // Arrange
            string path0 = Path.Combine(Path.GetTempPath(), "test.docx");
            string path1 = Path.Combine(Path.GetTempPath(), "test2.docx");
            DocxFilePath docx0 = DocxFilePath.Create(path0);
            DocxFilePath docx1 = DocxFilePath.Create(path1);
            DocxFilePath[] files = new DocxFilePath[] { docx0, docx1 };

            // Act
            IEnumerable<XElement> elements = files.ReadAsXml();

            // Assert
            Assert.IsTrue(elements.All(x => x != null));
        }

        [Theory]
        public void ReadAsXmlTest3()
        {
            // Arrange
            string path0 = Path.Combine(Path.GetTempPath(), "test.docx");
            string path1 = Path.Combine(Path.GetTempPath(), "test2.docx");
            DocxFilePath docx0 = DocxFilePath.Create(path0);
            DocxFilePath docx1 = DocxFilePath.Create(path1);
            DocxFilePath[] files = new DocxFilePath[] { docx0, docx1 };

            // Act
            IEnumerable<XElement> elements = files.AsParallel().ReadAsXml();

            // Assert
            Assert.IsTrue(elements.All(x => x != null));
        }
    }
}