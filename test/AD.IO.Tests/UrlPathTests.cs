using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AD.IO.Tests
{
    [TestClass]
    public class UrlPathTests
    {
        [Theory]
        public void UrlPathTest0()
        {
            // Arrange
            string path = Path.GetTempFileName();
            FilePath filePath = path;
            Uri uri = new Uri(path);

            // Act
            UrlPath url = (UrlPath) filePath;

            // Assert
            Assert.IsTrue(url.UriPath.AbsoluteUri == uri.AbsoluteUri);
        }

        [Theory]
        public void UrlPathTest1()
        {
            // Arrange
            string path = Path.GetTempFileName();
            FilePath filePath = path;

            // Act
            UrlPath url = (UrlPath)filePath;

            // Assert
            Assert.IsTrue(url.Name == null);
        }

        [Theory]
        public void UrlPathTest2()
        {
            // Arrange
            string path = Path.GetTempFileName();
            FilePath filePath = path;

            // Act
            UrlPath url = (UrlPath)filePath;

            // Assert
            Assert.IsTrue(url.Extension == null);
        }

        [Theory]
        public void UrlPathTest3()
        {
            // Arrange
            string path = Path.GetTempFileName();
            Uri uri = new Uri(path);

            // Act
            UrlPath urlPath = UrlPath.Create(uri.AbsoluteUri);

            // Assert
            Assert.AreEqual(uri.AbsoluteUri, urlPath.ToString());
        }

        [Theory]
        [ExpectedException(typeof(UriFormatException))]
        public void UrlPathTest4()
        {
            // Arrange
            const string path = "./&?><";

            // Act
            // ReSharper disable once UnusedVariable
            UrlPath urlPath = UrlPath.Create(path);
        }

        [Theory]
        public void UrlPathTest5()
        {
            // Arrange
            string path = Path.GetTempFileName();
            Uri uri = new Uri(path);
            IPath url = new UrlPath(path);

            // Act
            UrlPath urlPath = (UrlPath) url.Create(uri.AbsoluteUri);

            // Assert
            Assert.AreEqual(uri.AbsoluteUri, urlPath.ToString());
        }

        [Theory]
        public void UrlPathTest6()
        {
            // Arrange
            UrlPath urlPath = new Uri(Path.GetTempFileName()).AbsoluteUri;

            // Act
            IEnumerable<char> charPath = urlPath.Select(x => x);

            // Assert
            Assert.IsTrue(string.Join(null, charPath).Equals(urlPath.ToString()));
        }

        [Theory]
        public void UrlPathTest7()
        {
            // Arrange
            UrlPath urlPath = new Uri(Path.GetTempFileName()).AbsoluteUri;
            IEnumerable enumerable = urlPath.AsEnumerable();

            // Act
            bool test = enumerable.GetEnumerator().MoveNext();

            // Assert
            Assert.IsTrue(test);
        }

        [Theory]
        public void UrlPathTest8()
        {
            // Arrange
            UrlPath urlPath = new Uri(Path.GetTempFileName()).AbsoluteUri;

            // Act
            Uri test = urlPath;

            // Assert
            Assert.IsTrue(test != null);
        }

        [Theory]
        public void UrlPathTest9()
        {
            // Arrange
            UrlPath urlPath = new Uri(Path.GetTempFileName()).AbsoluteUri;

            // Act
            FilePath test = (FilePath)urlPath;

            // Assert
            Assert.AreEqual(urlPath.UriPath.AbsolutePath, test.ToString());
        }

        [Theory]
        public void UrlPathTest10()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string zip = path.Replace(".tmp", ".zip");
            File.Move(path, zip);
            UrlPath urlPath = new Uri(zip).AbsoluteUri;

            // Act
            ZipFilePath test = (ZipFilePath)urlPath;

            // Assert
            Assert.AreEqual(urlPath.UriPath.AbsolutePath, test.ToString());
        }

        [Theory]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UrlPathTest11()
        {
            // Arrange
            UrlPath urlPath = "https://www.google.com";

            // Act
            // ReSharper disable once UnusedVariable
            FilePath test = (FilePath)urlPath;
        }

        [Theory]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UrlPathTest12()
        {
            // Arrange
            UrlPath urlPath = "https://www.google.com";

            // Act
            // ReSharper disable once UnusedVariable
            ZipFilePath test = (ZipFilePath)urlPath;
        }
    }
}
