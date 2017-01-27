using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.IO
{
    [PublicAPI]
    public static class WriteXmlExtensions
    {
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

        public static void WriteXml(this IEnumerable<XElement> elements, XmlFilePath xmlFilePath, string completedMessage, SaveOptions saveOptions = SaveOptions.None, bool overwrite = true)
        {
            elements.WriteXml(xmlFilePath, saveOptions, overwrite);
            Console.WriteLine(completedMessage, DateTime.Now.TimeOfDay);
        }
    }
}
