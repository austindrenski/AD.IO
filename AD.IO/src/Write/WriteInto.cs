using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Extension methods to save XML content into the <see cref="ZipArchive"/> of a <see cref="DocxFilePath"/>.
    /// </summary>
    [PublicAPI]
    public static class WriteIntoExtensions
    {
        /// <summary>
        /// Saves the <paramref name="element"/> into the <paramref name="toFilePath"/> at the <paramref name="entryPath"/>.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/> that is written.</param>
        /// <param name="toFilePath">The file into which the <see cref="XElement"/> is written.</param>
        /// <param name="entryPath">The location to which the <see cref="XElement"/> is written.</param>
        public static void WriteInto(this XElement element, DocxFilePath toFilePath, string entryPath)
        {
            element.DescendantsAndSelf().Attributes("fileName").Remove();
            using (ZipArchive file = ZipFile.Open(toFilePath, ZipArchiveMode.Update))
            {
                file.GetEntry(entryPath)?.Delete();
                using (StreamWriter writer = new StreamWriter(file.CreateEntry(entryPath).Open()))
                {
                    //writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                    //writer.WriteLine(element);
                    element.Save(writer);
                }
            }
        }

        /// <summary>
        /// Saves the <paramref name="fromFilePath"/> into the <paramref name="toFilePath"/> at the <paramref name="entryPath"/>.
        /// </summary>
        /// <param name="fromFilePath">The file that is copied.</param>
        /// <param name="toFilePath">The file into which <paramref name="fromFilePath"/> is copied.</param>
        /// <param name="entryPath">The location to which the <paramref name="toFilePath"/> is copied.</param>
        public static void WriteInto(this DocxFilePath fromFilePath, DocxFilePath toFilePath, string entryPath)
        {
            string temp = Path.GetTempFileName();
            using (ZipArchive fromFile = ZipFile.Open(fromFilePath, ZipArchiveMode.Read))
            {
                fromFile.GetEntry(entryPath).ExtractToFile(temp, true);
            }
            using (ZipArchive toFile = ZipFile.Open(toFilePath, ZipArchiveMode.Update))
            {
                toFile.CreateEntryFromFile(temp, entryPath);
            }
        }

        /// <summary>
        /// Saves the file at <param name="fromEntryPath"/> in <paramref name="fromFilePath"/> into <param name="toEntryPath"/> in <paramref name="toFilePath"/>.
        /// </summary>
        /// <param name="fromFilePath">The file that is copied.</param>
        /// <param name="toFilePath">The file into which <paramref name="fromFilePath"/> is copied.</param>
        /// <param name="fromEntryPath">The location that is copied to the <paramref name="toEntryPath"/>.</param>
        /// <param name="toEntryPath">The location to which the <paramref name="fromEntryPath"/> is copied.</param>
        public static void WriteInto(this DocxFilePath fromFilePath, DocxFilePath toFilePath, string fromEntryPath, string toEntryPath)
        {
            string temp = Path.GetTempFileName();
            using (ZipArchive fromFile = ZipFile.Open(fromFilePath, ZipArchiveMode.Read))
            {
                fromFile.GetEntry(fromEntryPath).ExtractToFile(temp, true);
            }
            using (ZipArchive toFile = ZipFile.Open(toFilePath, ZipArchiveMode.Update))
            {
                toFile.CreateEntryFromFile(temp, toEntryPath);
            }
        }

        /// <summary>
        /// Saves the <paramref name="fromFilePath"/> into the <paramref name="toFilePath"/> at 'word/[name][extension]'.
        /// </summary>
        /// <param name="fromFilePath">The file that is copied.</param>
        /// <param name="toFilePath">The file into which <paramref name="fromFilePath"/> is copied.</param>
        public static void WriteInto(this DocxFilePath fromFilePath, DocxFilePath toFilePath)
        {
            using (ZipArchive toFile = ZipFile.Open(toFilePath, ZipArchiveMode.Update))
            {
                toFile.CreateEntryFromFile(fromFilePath, $"word/{fromFilePath.Name}{fromFilePath.Extension}");
            }
        }
    }
}