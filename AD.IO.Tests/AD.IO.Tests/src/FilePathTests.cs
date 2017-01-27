using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            FilePath filePath = path;

            // Assert
            Assert.IsFalse(filePath == path);
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
            Assert.IsFalse(filePath == path);
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
            FilePath filePath = new FilePath(path);

            // Assert
            Assert.IsFalse(filePath == path);
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
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            // Act
            FilePath filePath = FilePath.Create(path);

            // Assert
            Assert.IsTrue(filePath.ToString() == path);
        }
    }
}
