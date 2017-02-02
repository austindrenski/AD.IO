using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Extension methods to write <see cref="XElement"/> objects to HTML files.
    /// </summary>
    [PublicAPI]
    public static class WriteHtmlExtensions
    {
        /// <summary>
        /// Writes the &lt;html&gt; as "&lt;!DOCTYPE html&gt;&lt;html&gt;...&lt;/html&gt;" to the <see cref="HtmlFilePath"/>.
        /// </summary>
        /// <param name="element">The HTML element to be written to the <see cref="HtmlFilePath"/>.</param>
        /// <param name="htmlFilePath">The HTML file path to which the element is written.</param>
        /// <param name="overwrite">Overwrite if the file already exists.</param>
        public static void WriteHtml(this XElement element, HtmlFilePath htmlFilePath, bool overwrite = true)
        {
            if (!overwrite)
            {
                return;
            }
            using (FileStream stream = new FileStream(htmlFilePath, FileMode.Truncate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("<!DOCTYPE html>");
                    writer.WriteLine(element);
                }
            }
        }

        /// <summary>
        /// Wraps the elements in an &lt;html&gt; tag and writes "&lt;!DOCTYPE html&gt;&lt;html&gt;...&lt;/html&gt;" to the <see cref="HtmlFilePath"/>.
        /// </summary>
        /// <param name="elements">The HTML elements to be written to the <see cref="HtmlFilePath"/>.</param>
        /// <param name="htmlFilePath">The HTML file path to which the elements are written.</param>
        /// <param name="overwrite">Overwrite if the file already exists.</param>
        public static void WriteHtml(this IEnumerable<XElement> elements, HtmlFilePath htmlFilePath, bool overwrite = true)
        {
            XElement html = new XElement("html", elements);
            html.WriteHtml(htmlFilePath, overwrite);
        }

        /// <summary>
        /// Writes the &lt;html&gt; as "&lt;!DOCTYPE html&gt;&lt;html&gt;...&lt;/html&gt;" to the <see cref="HtmlFilePath"/>.
        /// </summary>
        /// <param name="element">The HTML element to be written to the <see cref="HtmlFilePath"/>.</param>
        /// <param name="htmlFilePath">The HTML file path to which the element is written.</param>
        /// <param name="completedMessage">A message written to SDTOUT upon completion. If present, {0} is replaced with the current DateTime.Now.TimeOfDay</param>
        /// <param name="overwrite">Overwrite if the file already exists.</param>
        public static void WriteHtml(this XElement element, HtmlFilePath htmlFilePath, string completedMessage, bool overwrite = true)
        {
            element.WriteHtml(htmlFilePath, overwrite);
            Console.WriteLine(completedMessage, DateTime.Now.TimeOfDay);
        }

        /// <summary>
        /// Writes the enumerable of HTML elements as children of &lt;html&gt;...&lt;/html&gt; to the <see cref="HtmlFilePath"/>.
        /// </summary>
        /// <param name="elements">The HTML elements to be written to the <see cref="HtmlFilePath"/>.</param>
        /// <param name="htmlFilePath">The HTML file path to which the elements are written.</param>
        /// <param name="completedMessage">A message written to SDTOUT upon completion. If present, {0} is replaced with the current DateTime.Now.TimeOfDay</param>
        /// <param name="overwrite">Overwrite if the file already exists.</param>
        public static void WriteHtml(this IEnumerable<XElement> elements, HtmlFilePath htmlFilePath, string completedMessage, bool overwrite = true)
        {
            elements.WriteHtml(htmlFilePath, overwrite);
            Console.WriteLine(completedMessage, DateTime.Now.TimeOfDay);
        }

    }
}
