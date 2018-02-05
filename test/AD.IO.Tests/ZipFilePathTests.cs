﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AD.IO.Tests
{
    public class ZipFilePathTests
    {
        [Theory]
        public void ZipFilePathTest0()
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

        [Theory]
        public void ZipFilePathTest1()
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

        [Theory]
        public void ZipFilePathTest2()
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

        [Theory]
        public void ZipFilePathTest3()
        {
            // Arrange
            string path = Path.GetTempFileName();

            // Act
            Assert.Throws<ArgumentException>(() => (ZipFilePath) path);
        }

        [Theory]
        public void ZipFilePathTest4()
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

        [Theory]
        public void ZipFilePathTest5()
        {
            // Arrange
            string path = Path.GetTempFileName();
            IPath iPath = new ZipFilePath(Path.ChangeExtension(path, ".zip"));

            // Act
            Assert.Throws<ArgumentException>(() => (ZipFilePath) iPath.Create(path));
        }

        [Theory]
        public void ZipFilePathTest6()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string test = path.Replace(".tmp", ".zip");

            // Act
            ZipFilePath zip = ZipFilePath.Create(test);

            // Assert
            Assert.Equal(zip.Extension, Path.GetExtension(test));
        }

        [Theory]
        public void ZipFilePathTest7()
        {
            // Arrange
            ZipFilePath zip = ZipFilePath.Create(Path.GetTempFileName().Replace(".tmp", ".zip"));

            // Act
            IEnumerable<char> charPath = zip.Select(x => x);

            // Assert
            Assert.True(string.Join(null, charPath).Equals(zip.ToString()));
        }

        [Theory]
        public void ZipFilePathTest8()
        {
            // Arrange
            ZipFilePath zip = ZipFilePath.Create(Path.GetTempFileName().Replace(".tmp", ".zip"));
            IEnumerable enumerable = zip.AsEnumerable();

            // Act
            bool test = enumerable.GetEnumerator().MoveNext();

            // Assert
            Assert.True(test);
        }

        [Theory]
        public void ZipFilePathTest9()
        {
            // Arrange
            ZipFilePath zip = ZipFilePath.Create(Path.GetTempFileName().Replace(".tmp", ".zip"));

            // Act
            string test = zip.ToString();

            // Assert
            Assert.True(test == zip);
        }

        [Theory]
        public void ZipFilePathTest10()
        {
            // Arrange
            UrlPath urlPath = new Uri(Path.GetTempFileName()).AbsoluteUri;

            // Act
            FilePath test = (FilePath) urlPath;

            // Assert
            Assert.Equal(urlPath.UriPath.AbsolutePath, test.ToString());
        }

        [Theory]
        public void ZipFilePathTest11()
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

        [Theory]
        public void ZipFilePathTest12()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string zip = path.Replace(".tmp", ".zip");
            IPath iPath = new ZipFilePath(zip);

            // Act
            ZipFilePath test = (ZipFilePath) iPath.Create(zip);

            // Assert
            Assert.Equal(zip, test.ToString());
        }

        [Theory]
        public void ZipFilePathTest13()
        {
            // Arrange
            ZipFilePath zip = ZipFilePath.Create(Path.GetTempFileName().Replace(".tmp", ".zip"));

            // Act
            UrlPath test = (UrlPath) zip;

            // Act
            Assert.Equal(zip.ToString().Replace('\\', '/'), test.UriPath.AbsolutePath);
        }

        [Theory]
        public void ZipFilePathTest14()
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