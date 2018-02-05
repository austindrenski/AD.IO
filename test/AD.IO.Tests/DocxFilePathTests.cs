using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AD.IO.Paths;
using Xunit;

namespace AD.IO.Tests
{
    public class DocxFilePathTests
    {
        [Fact]
        public void DocxFilePathTest0()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string test = path.Replace(".tmp", ".zip");
            File.Move(path, test);

            // Act
            ZipFilePath zip = test;

            // Assert
            Assert.Equal(zip.Name, Path.GetFileName(test).Replace(".zip", null));
        }

        [Fact]
        public void DocxFilePathTest1()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string test = path.Replace(".tmp", ".zip");
            File.Move(path, test);

            // Act
            ZipFilePath zip = test;

            // Assert
            Assert.Equal(zip.Extension, Path.GetExtension(test));
        }

        [Fact]
        public void DocxFilePathTest2()
        {
            // Arrange
            const string path = "test";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            // Act
            Assert.Throws<FileNotFoundException>(() => new ZipFilePath(path));
        }

        [Fact]
        public void DocxFilePathTest3()
        {
            // Arrange
            string path = Path.GetTempFileName();

            // Act
            Assert.Throws<ArgumentException>(() => (ZipFilePath) path);
        }

        [Fact]
        public void DocxFilePathTest4()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string test = path.Replace(".tmp", ".zip");
            File.Move(path, test);

            // Act
            ZipFilePath zip = ZipFilePath.Create(test);

            // Assert
            Assert.Equal(zip.Extension, Path.GetExtension(test));
        }

        [Fact]
        public void DocxFilePathTest5()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string zip = path.Replace(".tmp", ".zip");
            File.Move(path, zip);
            IPath iPath = new ZipFilePath(zip);

            // Act
            Assert.Throws<ArgumentException>(() => (ZipFilePath) iPath.Create(path));
        }

        [Fact]
        public void DocxFilePathTest6()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string test = path.Replace(".tmp", ".zip");

            // Act
            ZipFilePath zip = ZipFilePath.Create(test);

            // Assert
            Assert.Equal(zip.Extension, Path.GetExtension(test));
        }

        [Fact]
        public void DocxFilePathTest7()
        {
            // Arrange
            ZipFilePath zip = ZipFilePath.Create(Path.GetTempFileName().Replace(".tmp", ".zip"));

            // Act
            IEnumerable<char> charPath = zip.Select(x => x);

            // Assert
            Assert.True(string.Join(null, charPath).Equals(zip.ToString()));
        }

        [Fact]
        public void DocxFilePathTest8()
        {
            // Arrange
            ZipFilePath zip = ZipFilePath.Create(Path.GetTempFileName().Replace(".tmp", ".zip"));
            IEnumerable enumerable = zip.AsEnumerable();

            // Act
            bool test = enumerable.GetEnumerator().MoveNext();

            // Assert
            Assert.True(test);
        }

        [Fact]
        public void DocxFilePathTest9()
        {
            // Arrange
            ZipFilePath zip = ZipFilePath.Create(Path.GetTempFileName().Replace(".tmp", ".zip"));

            // Act
            string test = zip.ToString();

            // Assert
            Assert.True(test == zip);
        }

        [Fact]
        public void DocxFilePathTest10()
        {
            // Arrange
            UrlPath urlPath = new Uri(Path.GetTempFileName()).AbsoluteUri;

            // Act
            FilePath test = (FilePath) urlPath;

            // Assert
            Assert.Equal(urlPath.UriPath.AbsolutePath, test.ToString());
        }

        [Fact]
        public void DocxFilePathTest11()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string zip = path.Replace(".tmp", ".zip");
            File.Move(path, zip);
            UrlPath urlPath = new Uri(zip).AbsoluteUri;

            // Act
            ZipFilePath test = (ZipFilePath) urlPath;

            // Assert
            Assert.Equal(urlPath.UriPath.AbsolutePath, test.ToString());
        }

        [Fact]
        public void DocxFilePathTest12()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string zip = path.Replace(".tmp", ".zip");
            File.Move(path, zip);
            IPath iPath = new ZipFilePath(zip);

            // Act
            ZipFilePath test = (ZipFilePath) iPath.Create(zip);

            // Assert
            Assert.Equal(zip, test.ToString());
        }

        [Fact]
        public void DocxFilePathTest13()
        {
            // Arrange
            ZipFilePath zip = ZipFilePath.Create(Path.GetTempFileName().Replace(".tmp", ".zip"));

            // Act
            UrlPath test = (UrlPath) zip;

            // Act
            Assert.Equal(zip.ToString().Replace('\\', '/'), test.UriPath.AbsolutePath);
        }

        [Fact]
        public void DocxFilePathTest14()
        {
            // Arrange
            ZipFilePath zip = ZipFilePath.Create(Path.GetTempFileName().Replace(".tmp", ".zip"));

            // Act
            FilePath test = (FilePath) zip;

            // Act
            Assert.Equal(zip.ToString(), test.ToString());
        }
    }
}