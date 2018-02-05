using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AD.IO.Tests
{
    [TestClass]
    public class ReadDataTests
    {
        [TestMethod]
        public void ReadDataTest0()
        {
            // Arrange
            string name = Path.ChangeExtension(Path.GetTempFileName(), ".csv");
            using (StreamWriter writer = new StreamWriter(name))
            {
                writer.WriteLine("ElasticityOfSubstitution,InitialPrice,MarketShare,Tariff");
                writer.WriteLine("4,0.9764852913975930,0.0164876157540142,0.0435080979193930");
                writer.WriteLine("4,1.0000000000000000,0.1826886798599640,0.0000000000000000");
                writer.WriteLine("4,1.0000000000000000,0.0747428059044746,0.0000000000000000");
                writer.WriteLine("4,1.0000000000000000,0.7260808984815470,0.0000000000000000");
            }
            DelimitedFilePath dataFile = DelimitedFilePath.Create(name, ',');

            // Act
            IDictionary<string, string[]> data = dataFile.ReadData();

            // Assert
            Assert.IsTrue(data["ElasticityOfSubstitution"].All(x => double.Parse(x) > 0.0));
        }
    }
}
