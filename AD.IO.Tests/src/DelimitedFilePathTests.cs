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
    public class DelimitedFilePathTests
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void DelimitedFilePathTest0()
        {
            // Arrange
            string path = Path.GetTempPath();

            // Act
            // ReSharper disable once UnusedVariable
            DelimitedFilePath filePath = path;

            // Assert
            // Expected exception: FileNotFoundException
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void DelimitedFilePathTest1()
        {
            // Arrange
            const string path = "";

            // Act
            DelimitedFilePath filePath = path;

            // Assert
            // Expected exception: FileNotFoundException
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void DelimitedFilePathTest2()
        {
            // Arrange
            string path = Path.GetTempPath();

            // Act
            // ReSharper disable once UnusedVariable
            DelimitedFilePath filePath = new DelimitedFilePath(path, '|');

            // Assert
            // Expected exception: FileNotFoundException
        }

        [TestMethod]
        public void DelimitedFilePathTest3()
        {
            // Arrange
            string path = Path.GetTempFileName();
            string test = path.Replace(".tmp", ".csv");
            File.Move(path, test);

            // Act
            DelimitedFilePath filePath = new DelimitedFilePath(test, '|');

            // Assert
            Assert.IsTrue(filePath.ToString() == test);
        }

        [TestMethod]
        public void DelimitedFilePathTest4()
        {
            // Arrange 
            string path = Path.Combine(Path.GetTempPath(), "testdirectory", ".csv");

            // Act 
            DelimitedFilePath filePath = DelimitedFilePath.Create(path, '|');

            // Assert
            Assert.IsTrue(filePath.ToString() == path);
        }

        [TestMethod]
        public void DelimitedFilePathTest5()
        {
            // Arrange 
            string path = Path.Combine(Path.GetTempPath(), "test.csv");

            // Act 
            DelimitedFilePath filePath = DelimitedFilePath.Create(path, '|');

            // Assert
            Assert.AreEqual(filePath.Extension, ".csv");
        }

        [TestMethod]
        public void DelimitedFilePathTest6()
        {
            // Arrange 
            string path = Path.Combine(Path.GetTempPath(), "test.csv");

            // Act 
            DelimitedFilePath filePath = DelimitedFilePath.Create(path, '|');

            // Assert
            Assert.AreEqual(filePath.Name, "test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DelimitedFilePathTest7()
        {
            // Arrange 
            string path = Path.Combine(Path.GetTempPath(), "test.txt");

            // Act 
            // ReSharper disable once UnusedVariable
            DelimitedFilePath filePath = DelimitedFilePath.Create(path, '|');
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DelimitedFilePathTest8()
        {
            // Arrange 
            string path = Path.GetTempFileName();

            // Act 
            // ReSharper disable once UnusedVariable
            DelimitedFilePath filePath = new DelimitedFilePath(path, '|');
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DelimitedFilePathTest9()
        {
            // Arrange 
            string path = Path.GetTempFileName().Replace(".temp", ".txt");

            // Act 
            // ReSharper disable once UnusedVariable
            DelimitedFilePath filePath = new DelimitedFilePath(path, '|');
        }

        [TestMethod]
        public void DelimitedFilePathTest10()
        {
            // Arrange 
            string path = Path.Combine(Path.GetTempPath(), "test.csv");

            // Act 
            DelimitedFilePath filePath = DelimitedFilePath.Create(path, '|');
            IEnumerable<char> charPath = filePath.Select(x => x);

            // Assert
            Assert.IsTrue(string.Join(null, charPath).Equals(filePath.ToString()));
        }

        [TestMethod]
        public void DelimitedFilePathTest11()
        {
            // Arrange 
            string path = Path.Combine(Path.GetTempPath(), "test.csv");

            // Act 
            DelimitedFilePath filePath = DelimitedFilePath.Create(path, '|');
            IEnumerable enumerable = filePath.AsEnumerable();
            bool test = enumerable.GetEnumerator().MoveNext();

            // Assert
            Assert.IsTrue(test);
        }

        [TestMethod]
        public void DelimitedFilePathTest12()
        {
            // Arrange 
            string path = Path.Combine(Path.GetTempPath(), "test.csv");

            // Act 
            DelimitedFilePath filePath = DelimitedFilePath.Create(path, '|');

            // Assert
            Assert.AreEqual(filePath.ToString(), path);
        }

        [TestMethod]
        public void DelimitedFilePathTest13()
        {
            // Arrange 
            string path = Path.Combine(Path.GetTempPath(), "test.csv");

            // Act 
            DelimitedFilePath filePath = path;

            // Assert
            Assert.AreEqual(filePath.ToString(), path);
        }

        [TestMethod]
        public void DelimitedFilePathTest14()
        {
            // Arrange 
            string path = Path.Combine(Path.GetTempPath(), "test.csv");
            DelimitedFilePath delimitedPath = path;

            // Act 
            FilePath filePath = (FilePath) delimitedPath;

            // Assert
            Assert.AreEqual(filePath.ToString(), path);
        }

        [TestMethod]
        public void DelimitedFilePathTest15()
        {
            // Arrange 
            string path = Path.Combine(Path.GetTempPath(), "test.csv");
            DelimitedFilePath delimitedPath = path;

            // Act 
            UrlPath urlPath = (UrlPath)delimitedPath;

            // Assert
            Assert.IsTrue(urlPath != "");
        }

        [TestMethod]
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
            Assert.IsTrue(headers.SequenceEqual(new string[] { "a", "b", "c" }));
        }

        [TestMethod]
        public void DelimitedFilePathTest18()
        {
            // Arrange 
            string path = Path.Combine(Path.GetTempPath(), "test.csv");
            DelimitedFilePath delimitedPath = new DelimitedFilePath(path, ',');
            IPath iPath = delimitedPath;

            // Act 
            DelimitedFilePath test = (DelimitedFilePath)iPath.Create(path);

            // Assert
            Assert.AreEqual(test.ToString(), delimitedPath.ToString());
        }
    }
}