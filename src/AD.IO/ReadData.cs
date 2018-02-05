using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AD.IO.Paths;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Extension methods to import files as XML nodes.
    /// </summary>
    [PublicAPI]
    public static class ReadDataExtensions
    {
        /// <summary>
        /// Read the delimited file as a dictionary of column vectors stored by header string.
        /// </summary>
        /// <param name="delimitedFilePath">
        /// The file to read.
        /// </param>
        /// <returns>
        /// An <see cref="IDictionary{TKey, TValue}"/> where each column from the delimited file is stored as a key entry.
        /// </returns>
        [NotNull]
        public static IDictionary<string, string[]> ReadData([NotNull] this DelimitedFilePath delimitedFilePath)
        {
            if (delimitedFilePath is null)
            {
                throw new ArgumentNullException(nameof(delimitedFilePath));
            }

            string[] headers =
                delimitedFilePath.Headers as string[] ?? delimitedFilePath.Headers.ToArray();

            IDictionary<string, string[]> dictionary = new Dictionary<string, string[]>(headers.Length);
            
            string[][] lines = 
                File.ReadLines(delimitedFilePath)
                    .Skip(1)
                    .SplitDelimitedLine(delimitedFilePath.Delimiter)
                    .Select(x => x.ToArray())
                    .ToArray()
                    ?? 
                    new string[0][];
            
            foreach (string header in headers)
            {
                dictionary.Add(header, new string[lines.Length]);
            }

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < headers.Length; j++)
                {
                    dictionary[headers[j]][i] = lines[i][j];
                }
            }
            
            return dictionary;    
        }
    }
}