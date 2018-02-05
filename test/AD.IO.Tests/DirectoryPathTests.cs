﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AD.IO.Tests
{
    public class DirectoryPathTests
    {
        [Theory]
        public void DirectoryPathTest0()
        {
            // Arrange
            string path = Path.GetTempPath();

            // Act
            DirectoryPath directoryPath = path;
            string result = directoryPath;

            // Assert
            Assert.True(result == path);
        }

        [Theory]
        public void DirectoryPathTest1()
        {
            // Arrange
            string path = Path.GetTempFileName();

            // Act
            Assert.Throws<DirectoryNotFoundException>(() => (DirectoryPath) path);
        }

        [Theory]
        public void DirectoryPathTest2()
        {
            // Arrange
            const string path = "";

            // Act
            Assert.Throws<DirectoryNotFoundException>(() => (DirectoryPath) path);
        }

        [Theory]
        public void DirectoryPathTest3()
        {
            // Arrange
            string path = Path.GetTempPath();

            // Act
            DirectoryPath directoryPath = new DirectoryPath(path);

            // Assert
            Assert.True(directoryPath == path);
        }

        [Theory]
        public void DirectoryPathTest4()
        {
            // Arrange
            string path = Path.GetTempFileName();

            // Act
            Assert.Throws<DirectoryNotFoundException>(() => new DirectoryPath(path));
        }

        [Theory]
        public void DirectoryPathTest5()
        {
            // Arrange
            string path = Path.GetTempPath();

            // Act
            DirectoryPath directoryPath = new DirectoryPath(path);

            // Assert
            Assert.True(directoryPath.ToString() == path);
        }

        [Theory]
        public void DirectoryPathTest6()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "testdirectory");
            if (Directory.Exists(path))
            {
                Directory.EnumerateFiles(path).ToList().ForEach(File.Delete);
                Directory.Delete(path);
            }

            // Act
            DirectoryPath directoryPath = DirectoryPath.Create(path);

            // Assert
            Assert.True(directoryPath.ToString() == path);
        }

        [Theory]
        public void DirectoryPathTest7()
        {
            // Arrange
            string path = Path.Combine(Path.GetTempPath(), "testdirectory");
            DirectoryPath directoryPath = path;

            // Act
            string extension = directoryPath.Extension;

            // Assert
            Assert.True(extension == null);
        }

        [Theory]
        public void DirectoryPathTest8()
        {
            // Arrange
            DirectoryPath directoryPath = Path.GetTempPath();

            // Act
            string name = directoryPath.Name;

            // Assert
            Assert.True(name != null);
        }

        [Theory]
        public void DirectoryPathTest9()
        {
            // Arrange
            DirectoryPath directoryPath = new DirectoryPath(Path.GetTempPath());
            IPath path = directoryPath;

            // Act
            IPath test = path.Create(Path.GetTempPath());

            // Assert
            Assert.Equal(test.ToString(), directoryPath.ToString());
        }

        [Theory]
        public void DirectoryPathTest10()
        {
            // Arrange
            DirectoryPath directoryPath = Path.GetTempPath();

            // Act
            IEnumerable<char> charPath = directoryPath.Select(x => x);

            // Assert
            Assert.True(string.Join(null, charPath).Equals(directoryPath.ToString()));
        }

        [Theory]
        public void DirectoryPathTest11()
        {
            // Arrange
            DirectoryPath directoryPath = Path.GetTempPath();
            IEnumerable enumerable = directoryPath.AsEnumerable();

            // Act
            bool test = enumerable.GetEnumerator().MoveNext();

            // Assert
            Assert.True(test);
        }
    }
}