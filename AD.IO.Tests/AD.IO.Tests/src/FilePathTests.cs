using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        [TestMethod]
        public void FilePathTest7()
        {
            // Arrange 
            string path = Path.GetTempFileName();
            FilePath filePath = path;

            // Act
            string extension = filePath.Extension;

            // Assert
            Assert.IsTrue(extension == ".tmp");
        }

        [TestMethod]
        public void FilePathTest8()
        {
            // Arrange 
            string fileName = Path.GetTempFileName();
            FilePath filePath = fileName;

            // Act
            string name = filePath.Name;

            // Assert
            Assert.IsTrue(name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void FilePathTest9()
        {
            // Arrange 
            FilePath filePath;
            IPath path = filePath;
            string name = Path.GetTempFileName().Replace(".tmp", "0.tmp");

            // Act
            IPath test = path.Create(name);

            // Assert
            Assert.IsTrue(test.ToString().Equals(name, System.StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void FilePathTest10()
        {
            // Arrange 
            FilePath filePath = Path.GetTempFileName();

            // Act
            IEnumerable<char> charPath = filePath.Select(x => x);

            // Assert
            Assert.IsTrue(string.Join(null, charPath).Equals(filePath.ToString()));
        }

        [TestMethod]
        public void FilePathTest11()
        {
            // Arrange 
            FilePath filePath = Path.GetTempFileName();
            IEnumerable enumerable = filePath.AsEnumerable();

            // Act
            bool test = enumerable.GetEnumerator().MoveNext();

            // Assert
            Assert.IsTrue(test);
        }

        [TestMethod]
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
            Assert.IsTrue(result.Name == filePath.Name);
        }
        
        [TestMethod]
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
            Assert.IsTrue(result.Name == filePath.Name);
        }

        [TestMethod]
        public void FilePathTest14()
        {
            // Arrange 
            FilePath filePath = Path.GetTempFileName();

            // Act
            UrlPath result = (UrlPath)filePath;

            // Assert
            Assert.IsTrue(result != "");
        }
    }
}
