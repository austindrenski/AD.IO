using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using AD.Collections;

namespace AD.IO
{
    /// <summary>
    /// Extension methods to import files as XML nodes.
    /// </summary>
    [PublicAPI]
    public static class ReadDataExtensions
    {
        /// <summary>
        /// Read the delimited file as a dictionary of columns whose values are doubles.
        /// </summary>
        /// <param name="delimitedFilePath">The file to read.</param>
        /// <returns>An IDictionary where each column from the delimited file is stored as a key entry.</returns>
        public static IDictionary<string, double[]> ReadData(this DelimitedFilePath delimitedFilePath)
        {
            IDictionary<string, double[]> data = new Dictionary<string, double[]>();

            string[] headers = delimitedFilePath.Headers as string[] ?? delimitedFilePath.Headers.ToArray();

            double[][] lines = 
                File.ReadLines(delimitedFilePath)
                    .Skip(1)
                    .SplitDelimitedLine(',')?
                    .Select(x => x.Select(double.Parse))
                    .ToJaggedArray() ?? new double[0][];
            
            foreach (string header in delimitedFilePath.Headers)
            {
                data.Add(header, new double[lines.Length]);
            }

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < headers.Length; j++)
                {
                    data[headers[j]][j] = lines[i][j];
                }
            }
            
            return data;    
        }
    }
}
