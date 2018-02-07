using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Xml.Linq;
using AD.IO.Paths;
using AD.IO.Streams;
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
        /// Saves the <paramref name="fromStream"/> into the <paramref name="toStream"/> at the <paramref name="entryPath"/>.
        /// </summary>
        /// <param name="fromStream">The <see cref="XElement"/> that is written.</param>
        /// <param name="toStream">The file into which the <see cref="XElement"/> is written.</param>
        /// <param name="entryPath">The location to which the <see cref="XElement"/> is written.</param>
        public static async Task<MemoryStream> WriteInto([NotNull] this Stream fromStream, [NotNull] Stream toStream, [NotNull] string entryPath)
        {
            if (fromStream is null)
            {
                throw new ArgumentNullException(nameof(fromStream));
            }

            if (toStream is null)
            {
                throw new ArgumentNullException(nameof(toStream));
            }

            if (entryPath is null)
            {
                throw new ArgumentNullException(nameof(entryPath));
            }

            if (!fromStream.CanRead || !fromStream.CanSeek)
            {
                throw new InvalidOperationException(nameof(fromStream));
            }

            fromStream.Seek(0, SeekOrigin.Begin);

            MemoryStream result = await toStream.CopyPure();

            using (ZipArchive fromArchive = new ZipArchive(fromStream))
            {
                using (ZipArchive resultArchive = new ZipArchive(result))
                {
                    using (StreamReader reader = new StreamReader(fromArchive.GetEntry(entryPath).Open()))
                    {
                        using (StreamWriter writer = new StreamWriter(resultArchive.CreateEntry(entryPath).Open()))
                        {
                            await writer.WriteAsync(await reader.ReadToEndAsync());
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Saves the <paramref name="element"/> into the <paramref name="toStream"/> at the <paramref name="entryPath"/>.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/> that is written.</param>
        /// <param name="toStream">The file into which the <see cref="XElement"/> is written.</param>
        /// <param name="entryPath">The location to which the <see cref="XElement"/> is written.</param>
        [Pure]
        [NotNull]
        public static async Task<MemoryStream> WriteInto([NotNull] this XElement element, [NotNull] Stream toStream, [NotNull] string entryPath)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (toStream is null)
            {
                throw new ArgumentNullException(nameof(toStream));
            }

            if (entryPath is null)
            {
                throw new ArgumentNullException(nameof(entryPath));
            }

            MemoryStream result = await toStream.CopyPure();

            using (ZipArchive file = new ZipArchive(result, ZipArchiveMode.Update, true))
            {
                file.GetEntry(entryPath)?.Delete();
                using (StreamWriter writer = new StreamWriter(file.CreateEntry(entryPath).Open()))
                {
                    element.Save(writer);
                }
            }

            return result;
        }

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