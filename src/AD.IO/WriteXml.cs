using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using AD.IO.Paths;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Extension methods to write an <see cref="IEnumerable{XElement}"/> to an XML file.
    /// </summary>
    [PublicAPI]
    public static class WriteXmlExtensions
    {
        /// <summary>
        /// Writes the <see cref="IEnumerable{XElement}"/> as an XML file.
        /// </summary>
        /// <param name="elements">The source enumerable.</param>
        /// <param name="xmlFilePath">The file to which the content is written.</param>
        /// <param name="saveOptions">Specifies serialization options.</param>
        /// <param name="overwrite">True to overwrite an existing file.</param>
        public static void WriteXml(this IEnumerable<XElement> elements, XmlFilePath xmlFilePath, SaveOptions saveOptions = SaveOptions.None, bool overwrite = true)
        {
            if (!overwrite)
            {
                return;
            }
            using (FileStream stream = new FileStream(xmlFilePath, FileMode.Truncate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    XStreamingElement element = new XStreamingElement("root", elements);
                    element.Save(writer, saveOptions);
                }
            }
        }

        /// <summary>
        /// Writes the <see cref="IEnumerable{XElement}"/> as an XML file.
        /// </summary>
        /// <param name="elements">The source enumerable.</param>
        /// <param name="xmlFilePath">The file to which the content is written.</param>
        /// <param name="completedMessage">The message written to the console upon completion.</param>
        /// <param name="saveOptions">Specifies serialization options.</param>
        /// <param name="overwrite">True to overwrite an existing file.</param>
        public static void WriteXml(this IEnumerable<XElement> elements, XmlFilePath xmlFilePath, string completedMessage, SaveOptions saveOptions = SaveOptions.None, bool overwrite = true)
        {
            elements.WriteXml(xmlFilePath, saveOptions, overwrite);
            Console.WriteLine(completedMessage, DateTime.Now.TimeOfDay);
        }
    }
}
