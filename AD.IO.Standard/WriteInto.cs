using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.IO.Standard
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
        public static void WriteInto([NotNull] this XElement element, [NotNull] DocxFilePath toFilePath, [NotNull] string entryPath)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            if (toFilePath is null)
            {
                throw new ArgumentNullException(nameof(toFilePath));
            }
            if (entryPath is null)
            {
                throw new ArgumentNullException(nameof(entryPath));
            }

            element.DescendantsAndSelf().Attributes("fileName").Remove();
            using (ZipArchive file = ZipFile.Open(toFilePath, ZipArchiveMode.Update))
            {
                file.GetEntry(entryPath)?.Delete();
                using (StreamWriter writer = new StreamWriter(file.CreateEntry(entryPath).Open()))
                {
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
        public static void WriteInto([NotNull] this DocxFilePath fromFilePath, [NotNull] DocxFilePath toFilePath, [NotNull] string entryPath)
        {
            if (fromFilePath is null)
            {
                throw new ArgumentNullException(nameof(fromFilePath));
            }
            if (toFilePath is null)
            {
                throw new ArgumentNullException(nameof(toFilePath));
            }
            if (entryPath is null)
            {
                throw new ArgumentNullException(nameof(entryPath));
            }

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
        /// Saves a doucment part from one file into another file at the given path.
        /// </summary>
        /// <param name="fromFilePath">The file that is copied.</param>
        /// <param name="toFilePath">The file into which <paramref name="fromFilePath"/> is copied.</param>
        /// <param name="fromEntryPath">The location that is copied to the file.</param>
        /// <param name="toEntryPath">The location to which the file is copied.</param>
        public static void WriteInto([NotNull] this DocxFilePath fromFilePath, [NotNull] DocxFilePath toFilePath, [NotNull] string fromEntryPath, [NotNull] string toEntryPath)
        {
            if (fromFilePath is null)
            {
                throw new ArgumentNullException(nameof(fromFilePath));
            }
            if (toFilePath is null)
            {
                throw new ArgumentNullException(nameof(toFilePath));
            }
            if (fromEntryPath is null)
            {
                throw new ArgumentNullException(nameof(fromEntryPath));
            }
            if (toEntryPath is null)
            {
                throw new ArgumentNullException(nameof(toEntryPath));
            }

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
        public static void WriteInto([NotNull] this DocxFilePath fromFilePath, [NotNull] DocxFilePath toFilePath)
        {
            if (fromFilePath is null)
            {
                throw new ArgumentNullException(nameof(fromFilePath));
            }
            if (toFilePath is null)
            {
                throw new ArgumentNullException(nameof(toFilePath));
            }

            using (ZipArchive toFile = ZipFile.Open(toFilePath, ZipArchiveMode.Update))
            {
                toFile.CreateEntryFromFile(fromFilePath, $"word/{fromFilePath.Name}{fromFilePath.Extension}");
            }
        }
    }
}