using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#pragma warning disable 219

namespace AD.IO.Tests
{
    [TestClass]
    public class DirectoryPathTests
    {
        [TestMethod]
        public void DirectoryPathTest0()
        {
            // Arrange
            string path = Path.GetTempPath();

            // Act
            DirectoryPath directoryPath = path;
            string result = directoryPath;

            // Assert
            Assert.IsTrue(result == path);
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void DirectoryPathTest1()
        {
            // Arrange
            string path = Path.GetTempFileName();

            // Act
            // ReSharper disable once UnusedVariable
            DirectoryPath directoryPath = path;

            // Assert
            // Expected exception: DirectoryNotFoundException
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void DirectoryPathTest2()
        {
            // Arrange
            const string path = "";

            // Act
            DirectoryPath directoryPath = path;

            // Assert
            // Expected exception: DirectoryNotFoundException
        }

        [TestMethod]
        public void DirectoryPathTest3()
        {
            // Arrange
            string path = Path.GetTempPath();

            // Act
            DirectoryPath directoryPath = new DirectoryPath(path);

            // Assert
            Assert.IsTrue(directoryPath == path);
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void DirectoryPathTest4()
        {
            // Arrange
            string path = Path.GetTempFileName();

            // Act
            // ReSharper disable once UnusedVariable
            DirectoryPath directoryPath = new DirectoryPath(path);

            // Assert
            // Expected exception: DirectoryNotFoundException
        }

        [TestMethod]
        public void DirectoryPathTest5()
        {
            // Arrange
            string path = Path.GetTempPath();

            // Act
            DirectoryPath directoryPath = new DirectoryPath(path);

            // Assert
            Assert.IsTrue(directoryPath.ToString() == path);
        }

        [TestMethod]
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
            Assert.IsTrue(directoryPath.ToString() == path);
        }
    }
}
