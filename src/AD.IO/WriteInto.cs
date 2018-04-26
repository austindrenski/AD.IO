using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
        /// <param name="fromStream">
        /// The <see cref="XElement"/> that is written.
        /// </param>
        /// <param name="toStream">
        /// The file into which the <see cref="XElement"/> is written.
        /// </param>
        /// <param name="entryPath">
        /// The location to which the <see cref="XElement"/> is written.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="MemoryStream"/> with the contents of the original written to the path.
        /// </returns>
        [NotNull]
        [ItemNotNull]
        public static async Task<MemoryStream> WriteIntoAsync([NotNull] this Task<Stream> fromStream, [NotNull] Stream toStream, [NotNull] string entryPath)
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

            return await WriteIntoAsync(await fromStream, toStream, entryPath);
        }

        /// <summary>
        /// Saves the <paramref name="fromStream"/> into the <paramref name="toStream"/> at the <paramref name="entryPath"/>.
        /// </summary>
        /// <param name="fromStream">
        /// The <see cref="XElement"/> that is written.
        /// </param>
        /// <param name="toStream">
        /// The file into which the <see cref="XElement"/> is written.
        /// </param>
        /// <param name="entryPath">
        /// The location to which the <see cref="XElement"/> is written.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="MemoryStream"/> with the contents of the original written to the path.
        /// </returns>
        [NotNull]
        [ItemNotNull]
        public static async Task<MemoryStream> WriteIntoAsync([NotNull] this Task<Stream> fromStream, [NotNull] Task<Stream> toStream, [NotNull] string entryPath)
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

            return await WriteIntoAsync(await fromStream, await toStream, entryPath);
        }

        /// <summary>
        /// Saves the <paramref name="fromStream"/> into the <paramref name="toStream"/> at the <paramref name="entryPath"/>.
        /// </summary>
        /// <param name="fromStream">
        /// The <see cref="XElement"/> that is written.
        /// </param>
        /// <param name="toStream">
        /// The file into which the <see cref="XElement"/> is written.
        /// </param>
        /// <param name="entryPath">
        /// The location to which the <see cref="XElement"/> is written.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="MemoryStream"/> with the contents of the original written to the path.
        /// </returns>
        [NotNull]
        [ItemNotNull]
        public static async Task<MemoryStream> WriteIntoAsync([NotNull] this Stream fromStream, [NotNull] Stream toStream, [NotNull] string entryPath)
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

            using (ZipArchive fromArchive = new ZipArchive(fromStream, ZipArchiveMode.Read, true))
            {
                using (ZipArchive resultArchive = new ZipArchive(result, ZipArchiveMode.Update, true))
                {
                    using (StreamReader reader = new StreamReader(fromArchive.GetEntry(entryPath)?.Open() ?? throw new FileNotFoundException(entryPath)))
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
        /// <param name="element">
        /// The <see cref="XElement"/> that is written.
        /// </param>
        /// <param name="toStream">
        /// The file into which the <see cref="XElement"/> is written.
        /// </param>
        /// <param name="entryPath">
        /// The location to which the <see cref="XElement"/> is written.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="MemoryStream"/> with the XML written to the path.
        /// </returns>
        [Pure]
        [NotNull]
        [ItemNotNull]
        public static async Task<MemoryStream> WriteIntoAsync([NotNull] this XElement element, [NotNull] Task<Stream> toStream, [NotNull] string entryPath)
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

            return await WriteIntoAsync(element, await toStream, entryPath);
        }

        /// <summary>
        /// Saves the <paramref name="element"/> into the <paramref name="toStream"/> at the <paramref name="entryPath"/>.
        /// </summary>
        /// <param name="element">
        /// The <see cref="XElement"/> that is written.
        /// </param>
        /// <param name="toStream">
        /// The file into which the <see cref="XElement"/> is written.
        /// </param>
        /// <param name="entryPath">
        /// The location to which the <see cref="XElement"/> is written.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="MemoryStream"/> with the XML written to the path.
        /// </returns>
        [Pure]
        [NotNull]
        [ItemNotNull]
        public static async Task<MemoryStream> WriteIntoAsync([NotNull] this XElement element, [NotNull] Stream toStream, [NotNull] string entryPath)
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
        /// <param name="element">
        /// The <see cref="XElement"/> that is written.
        /// </param>
        /// <param name="toFilePath">
        /// The file into which the <see cref="XElement"/> is written.
        /// </param>
        /// <param name="entryPath">
        /// The location to which the <see cref="XElement"/> is written.
        /// </param>
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
        /// <param name="fromFilePath">
        /// The file that is copied.
        /// </param>
        /// <param name="toFilePath">
        /// The file into which <paramref name="fromFilePath"/> is copied.
        /// </param>
        /// <param name="entryPath">
        /// The location to which the <paramref name="toFilePath"/> is copied.
        /// </param>
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
        /// <param name="fromFilePath">
        /// The file that is copied.</param>
        /// <param name="toFilePath">
        /// The file into which <paramref name="fromFilePath"/> is copied.
        /// </param>
        /// <param name="fromEntryPath">
        /// The location that is copied to the file.
        /// </param>
        /// <param name="toEntryPath">
        /// The location to which the file is copied.
        /// </param>
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
        /// <param name="fromFilePath">
        /// The file that is copied.
        /// </param>
        /// <param name="toFilePath">
        /// The file into which <paramref name="fromFilePath"/> is copied.
        /// </param>
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="archive"></param>
        /// <param name="entryPath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [Pure]
        [NotNull]
        public static ZipArchive WriteInto([NotNull] this byte[] data, [NotNull] ZipArchive archive, [NotNull] string entryPath)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (archive is null)
            {
                throw new ArgumentNullException(nameof(archive));
            }

            if (entryPath is null)
            {
                throw new ArgumentNullException(nameof(entryPath));
            }

            return
                archive.Entries
                       .Aggregate(
                           new MemoryStream(),
                           (ms, entry) =>
                           {
                               using (Stream stream = entry.Open())
                               {
                                   stream.CopyTo(ms);
                               }

                               return ms;
                           },
                           result =>
                           {
                               ZipArchive zip = new ZipArchive(result, ZipArchiveMode.Update);

                               zip.GetEntry(entryPath)?.Delete();

                               using (Stream stream = zip.CreateEntry(entryPath).Open())
                               {
                                   stream.Write(data, default, data.Length);
                               }

                               return zip;
                           });
        }

        /// <summary>
        /// Writes the byte array into the stream at the <paramref name="entryPath"/>.
        /// </summary>
        /// <param name="data">
        /// The data to write.
        /// </param>
        /// <param name="stream">
        /// The stream into which the data is written.
        /// </param>
        /// <param name="entryPath">
        /// The path at which the data is written.
        /// </param>
        /// <returns>
        /// A new <see cref="MemoryStream"/> representing the input stream and the newly written data.
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        [Pure]
        [NotNull]
        public static MemoryStream WriteInto([NotNull] this byte[] data, [NotNull] Stream stream, [NotNull] string entryPath)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (entryPath is null)
            {
                throw new ArgumentNullException(nameof(entryPath));
            }

            return data.WriteIntoAsync(stream, entryPath).Result;
        }

        /// <summary>
        /// Writes the byte array into the stream at the <paramref name="entryPath"/>.
        /// </summary>
        /// <param name="data">
        /// The data to write.
        /// </param>
        /// <param name="stream">
        /// The stream into which the data is written.
        /// </param>
        /// <param name="entryPath">
        /// The path at which the data is written.
        /// </param>
        /// <returns>
        /// A new <see cref="MemoryStream"/> representing the input stream and the newly written data.
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        [Pure]
        [NotNull]
        [ItemNotNull]
        public static async Task<MemoryStream> WriteIntoAsync([NotNull] this byte[] data, [NotNull] Stream stream, [NotNull] string entryPath)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (entryPath is null)
            {
                throw new ArgumentNullException(nameof(entryPath));
            }

            MemoryStream memoryStream = await stream.CopyPure();

            using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Update, true))
            {
                ZipArchiveEntry entry = archive.GetEntry(entryPath);

                entry?.Delete();

                using (Stream newEntry = archive.CreateEntry(entryPath).Open())
                {
                    newEntry.Write(data, default, data.Length);
                }
            }

            return memoryStream;
        }
    }
}