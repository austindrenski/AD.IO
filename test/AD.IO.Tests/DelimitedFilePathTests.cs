using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AD.IO.Tests
{
    public class DelimitedFilePathTests
    {
        [Theory]
        public void DelimitedFilePathTest0()
        {
            // Arrange
            string path = Path.GetTempPath();

            // Act
            Assert.Throws<FileNotFoundException>(() => (DelimitedFilePath) path);
        }

        [Theory]
        public void DelimitedFilePathTest1()
        {
            // Arrange
            const string path = "";

            // Act
            Assert.Throws<FileNotFoundException>(() => (DelimitedFilePath) path);
        }

        [Theory]
        public void DelimitedFilePathTest2()
        {
            // Arrange
            string path = Path.GetTempPath();

            // Act
            Assert.Throws<FileNotFoundException>(() => (DelimitedFilePath) path);
        }

        [Theory]
        public void DelimitedFilePathTest3()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string test = path.Replace(".tmp", ".csv");
            File.Move(path, test);

            // Act
            DelimitedFilePath filePath = new DelimitedFilePath(test);

            // Assert
            Assert.True(filePath.ToString() == test);
        }

        [Theory]
        public void DelimitedFilePathTest4()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "testdirectory", ".csv");

            // Act
            DelimitedFilePath filePath = DelimitedFilePath.Create(path, '|');

            // Assert
            Assert.True(filePath.ToString() == path);
        }

        [Theory]
        public void DelimitedFilePathTest5()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "test.csv");

            // Act
            DelimitedFilePath filePath = DelimitedFilePath.Create(path, '|');

            // Assert
            Assert.Equal(filePath.Extension, ".csv");
        }

        [Theory]
        public void DelimitedFilePathTest6()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "test.csv");

            // Act
            DelimitedFilePath filePath = DelimitedFilePath.Create(path, '|');

            // Assert
            Assert.Equal(filePath.Name, "test");
        }

        [Theory]
        public void DelimitedFilePathTest7()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "test.txt");

            // Act
            Assert.Throws<ArgumentException>(() => DelimitedFilePath.Create(path, '|'));
        }

        [Theory]
        public void DelimitedFilePathTest8()
        {
            // Arrange
            string path = Path.GetTempFileName();

            // Act
            Assert.Throws<ArgumentException>(() => new DelimitedFilePath(path));
        }

        [Theory]
        public void DelimitedFilePathTest9()
        {
            // Arrange
            string path = Path.GetTempFileName().Replace(".temp", ".txt");

            // Act
            Assert.Throws<ArgumentException>(() => new DelimitedFilePath(path));
        }

        [Theory]
        public void DelimitedFilePathTest10()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "test.csv");

            // Act
            DelimitedFilePath filePath = DelimitedFilePath.Create(path, '|');
            IEnumerable<char> charPath = filePath.Select(x => x);

            // Assert
            Assert.True(string.Join(null, charPath).Equals(filePath.ToString()));
        }

        [Theory]
        public void DelimitedFilePathTest11()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "test.csv");

            // Act
            DelimitedFilePath filePath = DelimitedFilePath.Create(path, '|');
            IEnumerable enumerable = filePath.AsEnumerable();
            bool test = enumerable.GetEnumerator().MoveNext();

            // Assert
            Assert.True(test);
        }

        [Theory]
        public void DelimitedFilePathTest12()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "test.csv");

            // Act
            DelimitedFilePath filePath = DelimitedFilePath.Create(path, '|');

            // Assert
            Assert.Equal(filePath.ToString(), path);
        }

        [Theory]
        public void DelimitedFilePathTest13()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "test.csv");

            // Act
            DelimitedFilePath filePath = path;

            // Assert
            Assert.Equal(filePath.ToString(), path);
        }

        [Theory]
        public void DelimitedFilePathTest14()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "test.csv");
            DelimitedFilePath delimitedPath = path;

            // Act
            FilePath filePath = (FilePath) delimitedPath;

            // Assert
            Assert.Equal(filePath.ToString(), path);
        }

        [Theory]
        public void DelimitedFilePathTest15()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "test.csv");
            DelimitedFilePath delimitedPath = path;

            // Act
            UrlPath urlPath = (UrlPath) delimitedPath;

            // Assert
            Assert.True(urlPath != "");
        }

        [Theory]
        public void DelimitedFilePathTest17()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "test.csv");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("a,b,c");
                writer.WriteLine("1,2,3");
            }
            DelimitedFilePath delimitedPath = DelimitedFilePath.Create(path, ',');

            // Act
            IEnumerable<string> headers = delimitedPath.Headers;

            // Assert
            Assert.True(headers.SequenceEqual(new string[] { "a", "b", "c" }));
        }

        [Theory]
        public void DelimitedFilePathTest18()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "test.csv");
            DelimitedFilePath delimitedPath = new DelimitedFilePath(path, ',');
            IPath iPath = delimitedPath;

            // Act
            DelimitedFilePath test = (DelimitedFilePath) iPath.Create(path);

            // Assert
            Assert.Equal(test.ToString(), delimitedPath.ToString());
        }
    }
}