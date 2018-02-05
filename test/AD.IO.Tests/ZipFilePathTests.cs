using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#pragma warning disable 219

namespace AD.IO.Tests
{
    [TestClass]
    public class ZipFilePathTests
    {
        [TestMethod]
        public void ZipFilePathTest0()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string test = path.Replace(".tmp", ".zip");
            File.Move(path, test);

            // Act
            ZipFilePath zip = test;

            // Assert
            Assert.AreEqual(zip.Name, Path.GetFileName(test).Replace(".zip", null));
        }

        [TestMethod]
        public void ZipFilePathTest1()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string test = path.Replace(".tmp", ".zip");
            File.Move(path, test);

            // Act
            ZipFilePath zip = test;

            // Assert
            Assert.AreEqual(zip.Extension, Path.GetExtension(test));
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ZipFilePathTest2()
        {
            // Arrange
            const string path = "test";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            // Act
            // ReSharper disable once UnusedVariable
            ZipFilePath zip = new ZipFilePath(path);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ZipFilePathTest3()
        {
            // Arrange
            string path = Path.GetTempFileName();

            // Act
            // ReSharper disable once UnusedVariable
            ZipFilePath zip = path;
        }

        [TestMethod]
        public void ZipFilePathTest4()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string test = path.Replace(".tmp", ".zip");
            File.Move(path, test);

            // Act
            ZipFilePath zip = ZipFilePath.Create(test);

            // Assert
            Assert.AreEqual(zip.Extension, Path.GetExtension(test));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ZipFilePathTest5()
        {
            // Arrange
            string path = Path.GetTempFileName();
            IPath iPath = new ZipFilePath(Path.ChangeExtension(path, ".zip"));

            // Act
            // ReSharper disable once UnusedVariable
            ZipFilePath zip = (ZipFilePath) iPath.Create(path);
        }

        [TestMethod]
        public void ZipFilePathTest6()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string test = path.Replace(".tmp", ".zip");

            // Act
            ZipFilePath zip = ZipFilePath.Create(test);

            // Assert
            Assert.AreEqual(zip.Extension, Path.GetExtension(test));
        }

        [TestMethod]
        public void ZipFilePathTest7()
        {
            // Arrange 
            ZipFilePath zip = ZipFilePath.Create(Path.GetTempFileName().Replace(".tmp", ".zip"));

            // Act
            IEnumerable<char> charPath = zip.Select(x => x);

            // Assert
            Assert.IsTrue(string.Join(null, charPath).Equals(zip.ToString()));
        }

        [TestMethod]
        public void ZipFilePathTest8()
        {
            // Arrange 
            ZipFilePath zip = ZipFilePath.Create(Path.GetTempFileName().Replace(".tmp", ".zip"));
            IEnumerable enumerable = zip.AsEnumerable();

            // Act
            bool test = enumerable.GetEnumerator().MoveNext();

            // Assert
            Assert.IsTrue(test);
        }

        [TestMethod]
        public void ZipFilePathTest9()
        {
            // Arrange 
            ZipFilePath zip = ZipFilePath.Create(Path.GetTempFileName().Replace(".tmp", ".zip"));

            // Act
            string test = zip.ToString();

            // Assert
            Assert.IsTrue(test == zip);
        }

        [TestMethod]
        public void ZipFilePathTest10()
        {
            // Arrange 
            UrlPath urlPath = new Uri(Path.GetTempFileName()).AbsoluteUri;

            // Act
            FilePath test = (FilePath)urlPath;

            // Assert
            Assert.AreEqual(urlPath.UriPath.AbsolutePath, test.ToString());
        }

        [TestMethod]
        public void ZipFilePathTest11()
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

        [TestMethod]
        public void ZipFilePathTest12()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string zip = path.Replace(".tmp", ".zip");
            IPath iPath = new ZipFilePath(zip);

            // Act
            ZipFilePath test = (ZipFilePath)iPath.Create(zip);

            // Assert
            Assert.AreEqual(zip, test.ToString());
        }

        [TestMethod]
        public void ZipFilePathTest13()
        {
            // Arrange 
            ZipFilePath zip = ZipFilePath.Create(Path.GetTempFileName().Replace(".tmp", ".zip"));

            // Act
            UrlPath test = (UrlPath) zip;

            // Act
            Assert.AreEqual(zip.ToString().Replace('\\', '/'), test.UriPath.AbsolutePath);
        }

        [TestMethod]
        public void ZipFilePathTest14()
        {
            // Arrange 
            ZipFilePath zip = ZipFilePath.Create(Path.GetTempFileName().Replace(".tmp", ".zip"));

            // Act
            FilePath test = (FilePath)zip;

            // Act
            Assert.AreEqual(zip.ToString(), test.ToString());
        }
    }
}
