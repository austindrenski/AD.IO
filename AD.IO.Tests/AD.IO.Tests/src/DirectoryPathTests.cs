﻿using System.Collections;
using System.Collections.Generic;
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

        [TestMethod]
        public void DirectoryPathTest7()
        {
            // Arrange 
            string path = Path.Combine(Path.GetTempPath(), "testdirectory");
            DirectoryPath directoryPath = path;

            // Act
            string extension = directoryPath.Extension;

            // Assert
            Assert.IsTrue(extension == null);
        }

        [TestMethod]
        public void DirectoryPathTest8()
        {
            // Arrange 
            DirectoryPath directoryPath = Path.GetTempPath();

            // Act
            string name = directoryPath.Name;

            // Assert
            Assert.IsTrue(name.Equals("temp", System.StringComparison.OrdinalIgnoreCase));
        }
        
        [TestMethod]
        public void DirectoryPathTest9()
        {
            // Arrange 
            DirectoryPath directoryPath;
            IPath path = directoryPath;

            // Act
            IPath test = path.Create(Path.GetTempPath());

            // Assert
            Assert.IsTrue(test.Name.Equals("temp", System.StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void DirectoryPathTest10()
        {
            // Arrange 
            DirectoryPath directoryPath = Path.GetTempPath();

            // Act
            IEnumerable<char> charPath = directoryPath.Select(x => x);

            // Assert
            Assert.IsTrue(string.Join(null, charPath).Equals(directoryPath.ToString()));
        }

        [TestMethod]
        public void DirectoryPathTest11()
        {
            // Arrange 
            DirectoryPath directoryPath = Path.GetTempPath();
            IEnumerable enumerable = directoryPath.AsEnumerable();

            // Act
            bool test = enumerable.GetEnumerator().MoveNext();

            // Assert
            Assert.IsTrue(test);
        }
    }
}
