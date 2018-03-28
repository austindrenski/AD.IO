using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AD.IO.Paths;
using Xunit;

namespace AD.IO.Tests
{
    public class UrlPathTests
    {
        [Fact]
        public void UrlPathTest0()
        {
            // Arrange
            string path = Path.GetTempFileName();
            FilePath filePath = path;
            Uri uri = new Uri(path);

            // Act
            UrlPath url = (UrlPath) filePath;

            // Assert
            Assert.Equal(uri.AbsoluteUri, url.UriPath.AbsoluteUri);
        }

        [Fact]
        public void UrlPathTest1()
        {
            // Arrange
            string path = Path.GetTempFileName();
            FilePath filePath = path;

            // Act
            UrlPath url = (UrlPath) filePath;

            // Assert
            Assert.Null(url.Name);
        }

        [Fact]
        public void UrlPathTest2()
        {
            // Arrange
            string path = Path.GetTempFileName();
            FilePath filePath = path;

            // Act
            UrlPath url = (UrlPath) filePath;

            // Assert
            Assert.True(url.Extension == null);
        }

        [Fact]
        public void UrlPathTest3()
        {
            // Arrange
            string path = Path.GetTempFileName();
            Uri uri = new Uri(path);

            // Act
            UrlPath urlPath = UrlPath.Create(uri.AbsoluteUri);

            // Assert
            Assert.Equal(uri.AbsoluteUri, urlPath.ToString());
        }

        [Fact]
        public void UrlPathTest4()
        {
            // Arrange
            const string path = "./&?><";

            // Act
            Assert.Throws<UriFormatException>(() => UrlPath.Create(path));
        }

        [Fact]
        public void UrlPathTest5()
        {
            // Arrange
            string path = Path.GetTempFileName();
            Uri uri = new Uri(path);
            IPath url = new UrlPath(uri.AbsoluteUri);

            // Act
            UrlPath urlPath = (UrlPath) url.Create(uri.AbsoluteUri);

            // Assert
            Assert.Equal(uri.AbsoluteUri, urlPath.ToString());
        }

        [Fact]
        public void UrlPathTest6()
        {
            // Arrange
            UrlPath urlPath = new Uri(Path.GetTempFileName()).AbsoluteUri;

            // Act
            IEnumerable<char> charPath = urlPath.Select(x => x);

            // Assert
            Assert.Equal(string.Join(null, charPath), urlPath.ToString());
        }

        [Fact]
        public void UrlPathTest7()
        {
            // Arrange
            UrlPath urlPath = new Uri(Path.GetTempFileName()).AbsoluteUri;
            IEnumerable enumerable = urlPath.AsEnumerable();

            // Act
            bool test = enumerable.GetEnumerator().MoveNext();

            // Assert
            Assert.True(test);
        }

        [Fact]
        public void UrlPathTest8()
        {
            // Arrange
            UrlPath urlPath = new Uri(Path.GetTempFileName()).AbsoluteUri;

            // Act
            Uri test = urlPath;

            // Assert
            Assert.NotNull(test);
        }

        [Fact]
        public void UrlPathTest9()
        {
            // Arrange
            UrlPath urlPath = new Uri(Path.GetTempFileName()).AbsoluteUri;

            // Act
            FilePath test = (FilePath) urlPath;

            // Assert
            Assert.Equal(urlPath.UriPath.AbsolutePath, test.ToString());
        }

        [Fact]
        public void UrlPathTest10()
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
        public void UrlPathTest11()
        {
            // Arrange
            UrlPath urlPath = "https://www.google.com";

            // Act
            Assert.Throws<FileNotFoundException>(() => (FilePath) urlPath);
        }

        [Fact]
        public void UrlPathTest12()
        {
            // Arrange
            UrlPath urlPath = "https://www.google.com";

            // Act
            Assert.Throws<FileNotFoundException>(() => (ZipFilePath) urlPath);
        }
    }
}