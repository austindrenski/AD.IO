using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AD.IO.Paths;
using Xunit;

namespace AD.IO.Tests
{
    public class FilePathTests
    {
        [Fact]
        public void FilePathTest0()
        {
            // Arrange
            string path = Path.GetTempFileName();

            // Act
            FilePath filePath = path;

            // Assert
            Assert.Equal(path, filePath);
        }

        [Fact]
        public void FilePathTest1()
        {
            // Arrange
            string path = Path.GetTempPath();

            // Act
            Assert.Throws<FileNotFoundException>(() => (FilePath) path);
        }

        [Fact]
        public void FilePathTest2()
        {
            // Arrange
            const string path = "";

            // Act
            Assert.Throws<FileNotFoundException>(() => (FilePath) path);
        }

        [Fact]
        public void FilePathTest3()
        {
            // Arrange
            string path = Path.GetTempFileName();

            // Act
            FilePath filePath = new FilePath(path);

            // Assert
            Assert.Equal(path, filePath);
        }

        [Fact]
        public void FilePathTest4()
        {
            // Arrange
            string path = Path.GetTempPath();

            // Act
            Assert.Throws<FileNotFoundException>(() => new FilePath(path));
        }

        [Fact]
        public void FilePathTest5()
        {
            // Arrange
            string path = Path.GetTempFileName();

            // Act
            FilePath filePath = new FilePath(path);

            // Assert
            Assert.Equal(path, filePath.ToString());
        }

        [Fact]
        public void FilePathTest6()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "TestDirectory", "test.txt");

            // Act
            FilePath filePath = FilePath.Create(path);

            // Assert
            Assert.Equal(path, filePath.ToString());
        }

        [Fact]
        public void FilePathTest7()
        {
            // Arrange
            string path = Path.GetTempFileName();
            FilePath filePath = path;

            // Act
            string extension = filePath.Extension;

            // Assert
            Assert.Equal(".tmp", extension);
        }

        [Fact]
        public void FilePathTest8()
        {
            // Arrange
            string fileName = Path.GetTempFileName();
            FilePath filePath = fileName;

            // Act
            string name = filePath.Name;

            // Assert
            Assert.Contains(name, fileName, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void FilePathTest9()
        {
            // Arrange
            FilePath filePath = new FilePath(Path.GetTempFileName());
            IPath path = filePath;
            string name = Path.GetTempFileName().Replace(".tmp", "0.tmp");

            // Act
            IPath test = path.Create(name);

            // Assert
            Assert.Equal(name, test.ToString(), StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void FilePathTest10()
        {
            // Arrange
            FilePath filePath = Path.GetTempFileName();

            // Act
            IEnumerable<char> charPath = filePath.Select(x => x);

            // Assert
            Assert.Equal(string.Join(null, charPath), filePath.ToString());
        }

        [Fact]
        public void FilePathTest11()
        {
            // Arrange
            FilePath filePath = Path.GetTempFileName();
            IEnumerable enumerable = filePath.AsEnumerable();

            // Act
            bool test = enumerable.GetEnumerator().MoveNext();

            // Assert
            Assert.True(test);
        }

        [Fact]
        public void FilePathTest12()
        {
            // Arrange
            string tmp = Path.GetTempFileName();
            string name = tmp.Replace(".tmp", ".docx");
            File.Move(tmp, name);
            FilePath filePath = name;

            // Act
            DocxFilePath result = filePath;

            // Assert
            Assert.Equal(filePath.Name, result.Name);
        }

        [Fact]
        public void FilePathTest13()
        {
            // Arrange
            string tmp = Path.GetTempFileName();
            string name = tmp.Replace(".tmp", ".zip");
            File.Move(tmp, name);
            FilePath filePath = name;

            // Act
            ZipFilePath result = (ZipFilePath) filePath;

            // Assert
            Assert.Equal(filePath.Name, result.Name);
        }

        [Fact]
        public void FilePathTest14()
        {
            // Arrange
            FilePath filePath = Path.GetTempFileName();

            // Act
            UrlPath result = (UrlPath) filePath;

            // Assert
            Assert.NotEqual(string.Empty, result);
        }
    }
}