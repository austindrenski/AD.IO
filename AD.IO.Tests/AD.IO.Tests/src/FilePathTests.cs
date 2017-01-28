using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#pragma warning disable 219

namespace AD.IO.Tests
{
    [TestClass]
    public class FilePathTests
    {
        [TestMethod]
        public void FilePathTest0()
        {
            // Arrange
            string path = Path.GetTempFileName();

            // Act
            FilePath filePath = path;

            // Assert
            Assert.IsTrue(filePath == path);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FilePathTest1()
        {
            // Arrange
            string path = Path.GetTempPath();

            // Act
            // ReSharper disable once UnusedVariable
            FilePath filePath = path;

            // Assert
            // Expected exception: FileNotFoundException
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FilePathTest2()
        {
            // Arrange
            const string path = "";

            // Act
            FilePath filePath = path;

            // Assert
            // Expected exception: FileNotFoundException
        }

        [TestMethod]
        public void FilePathTest3()
        {
            // Arrange
            string path = Path.GetTempFileName();

            // Act
            FilePath filePath = new FilePath(path);

            // Assert
            Assert.IsTrue(filePath == path);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FilePathTest4()
        {
            // Arrange
            string path = Path.GetTempPath();

            // Act
            // ReSharper disable once UnusedVariable
            FilePath filePath = new FilePath(path);

            // Assert
            // Expected exception: FileNotFoundException
        }

        [TestMethod]
        public void FilePathTest5()
        {
            // Arrange
            string path = Path.GetTempFileName();

            // Act
            FilePath filePath = new FilePath(path);

            // Assert
            Assert.IsTrue(filePath.ToString() == path);
        }

        [TestMethod]
        public void FilePathTest6()
        {
            // Arrange 
            string path = Path.Combine(Path.GetTempPath(), "testdirectory", ".txt");

            // Act 
            FilePath filePath = FilePath.Create(path);

            // Assert
            Assert.IsTrue(filePath.ToString() == path);
        }
    }
}
