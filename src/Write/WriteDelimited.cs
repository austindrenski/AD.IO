using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.IO
{
    [PublicAPI]
    public static class WriteDelimitedExtensions
    {
        public static void WriteDelimited(this IEnumerable<XElement> elements, DelimitedFilePath filePath, string delimiter = "|", bool overwrite = true)
        {
            if (!overwrite)
            {
                return;
            }
            using (FileStream stream = new FileStream(filePath, FileMode.Truncate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(elements.ToDelimited(delimiter));
                }
            }
        }

        public static void WriteDelimited(this IEnumerable<XElement> elements, DelimitedFilePath filePath, string completedMessage, string delimiter = "|", bool overwrite = true)
        {
            elements.WriteDelimited(filePath, delimiter, overwrite);
            Console.WriteLine(completedMessage);
        }
    }
}