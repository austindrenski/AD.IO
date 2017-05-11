using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Extension methods to extract entries from a <see cref="ZipFilePath"/>.
    /// </summary>
    [PublicAPI]
    public static class ExtractZipFileExtensions
    {
        /// <summary>
        /// Extracts a single file from the <see cref="ZipFilePath"/>. An exception is thrown if more than one file is found in the zip archive.
        /// </summary>
        /// <param name="zipFilePath">The zip file to open.</param>
        /// <param name="filePath">The file path to which the extracted entry is saved.</param>
        /// <param name="overwrite">If true, the file path is overwritten.</param>
        /// <exception cref="InvalidOperationException"/>
        public static void ExtractZipFile(this ZipFilePath zipFilePath, FilePath filePath, bool overwrite)
        {
            if (File.Exists(filePath) && !overwrite)
            {
                return;
            }
            using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
            {
                archive.Entries.Single().ExtractToFile(filePath, true);
            }
        }

        /// <summary>
        /// Extracts a single file from the <see cref="ZipFilePath"/>. An exception is thrown if more than one <see cref="FilePath"/> is found in the zip archive.
        /// </summary>
        /// <param name="zipFilePath">The zip file to open.</param>
        /// <param name="filePath">The file path to which the extracted entry is saved.</param>
        /// <param name="overwrite">If true, the file path is overwritten.</param>
        /// <param name="completedMessage">The message written to STDOUT upon successful completion.</param>
        /// <exception cref="InvalidOperationException"/>
        public static void ExtractZipFile(this ZipFilePath zipFilePath, FilePath filePath, bool overwrite, string completedMessage)
        {
            zipFilePath.ExtractZipFile(filePath, overwrite);
            Console.WriteLine(completedMessage, DateTime.Now.TimeOfDay);
        }

        /// <summary>
        /// Extracts a single file from the <see cref="ZipFilePath"/>. An exception is thrown if more than one <see cref="ZipFilePath"/> is found in the zip archive.
        /// </summary>
        /// <param name="zipFilePath">The zip file to open.</param>
        /// <param name="outZipFilePath">The file path to which the extracted entry is saved.</param>
        /// <param name="overwrite">If true, the file path is overwritten.</param>
        /// <exception cref="InvalidOperationException"/>
        public static void ExtractZipFile(this ZipFilePath zipFilePath, ZipFilePath outZipFilePath, bool overwrite)
        {
            zipFilePath.ExtractZipFile((FilePath)outZipFilePath, overwrite);
        }

        /// <summary>
        /// Extracts a single file from the <see cref="ZipFilePath"/>. An exception is thrown if more than one <see cref="ZipFilePath"/> is found in the zip archive.
        /// </summary>
        /// <param name="zipFilePath">The zip file to open.</param>
        /// <param name="outZipFilePath">The file path to which the extracted entry is saved.</param>
        /// <param name="overwrite">If true, the file path is overwritten.</param>
        /// <param name="completedMessage">The message written to STDOUT upon successful completion.</param>
        /// <exception cref="InvalidOperationException"/>
        public static void ExtractZipFile(this ZipFilePath zipFilePath, ZipFilePath outZipFilePath, bool overwrite, string completedMessage)
        {
            zipFilePath.ExtractZipFile(outZipFilePath, overwrite);
            Console.WriteLine(completedMessage, DateTime.Now.TimeOfDay);
        }

        /// <summary>
        /// Extracts a single file from the <see cref="ZipFilePath"/>. An exception is thrown if more than one <see cref="DelimitedFilePath"/> is found in the zip archive.
        /// </summary>
        /// <param name="zipFilePath">The zip file to open.</param>
        /// <param name="delimitedFilePath">The file path to which the extracted entry is saved.</param>
        /// <param name="overwrite">If true, the file path is overwritten.</param>
        /// <exception cref="InvalidOperationException"/>
        public static void ExtractZipFile(this ZipFilePath zipFilePath, DelimitedFilePath delimitedFilePath, bool overwrite)
        {
            zipFilePath.ExtractZipFile((FilePath)delimitedFilePath, overwrite);
        }


        /// <summary>
        /// Extracts a single file from the <see cref="ZipFilePath"/>. An exception is thrown if more than one <see cref="DelimitedFilePath"/> is found in the zip archive.
        /// </summary>
        /// <param name="zipFilePath">The zip file to open.</param>
        /// <param name="delimitedFilePath">The file path to which the extracted entry is saved.</param>
        /// <param name="overwrite">If true, the file path is overwritten.</param>
        /// <param name="completedMessage">The message written to STDOUT upon successful completion.</param>
        /// <exception cref="InvalidOperationException"/>
        public static void ExtractZipFile(this ZipFilePath zipFilePath, DelimitedFilePath delimitedFilePath, bool overwrite, string completedMessage)
        {
            zipFilePath.ExtractZipFile((FilePath)delimitedFilePath, overwrite);
            Console.WriteLine(completedMessage, DateTime.Now.TimeOfDay);
        }

        /// <summary>
        /// Extracts a single file from the <see cref="ZipFilePath"/>. An exception is thrown if more than one <see cref="XmlFilePath"/> is found in the zip archive.
        /// </summary>
        /// <param name="zipFilePath">The zip file to open.</param>
        /// <param name="xmlFilePath">The file path to which the extracted entry is saved.</param>
        /// <param name="overwrite">If true, the file path is overwritten.</param>
        /// <exception cref="InvalidOperationException"/>
        public static void ExtractZipFile(this ZipFilePath zipFilePath, XmlFilePath xmlFilePath, bool overwrite)
        {
            zipFilePath.ExtractZipFile((FilePath)xmlFilePath, overwrite);
        }
        
        /// <summary>
        /// Extracts a single file from the <see cref="ZipFilePath"/>. An exception is thrown if more than one <see cref="XmlFilePath"/> is found in the zip archive.
        /// </summary>
        /// <param name="zipFilePath">The zip file to open.</param>
        /// <param name="xmlFilePath">The file path to which the extracted entry is saved.</param>
        /// <param name="overwrite">If true, the file path is overwritten.</param>
        /// <param name="completedMessage">The message written to STDOUT upon successful completion.</param>
        /// <exception cref="InvalidOperationException"/>
        public static void ExtractZipFile(this ZipFilePath zipFilePath, XmlFilePath xmlFilePath, bool overwrite, string completedMessage)
        {
            zipFilePath.ExtractZipFile((FilePath)xmlFilePath, overwrite);
            Console.WriteLine(completedMessage, DateTime.Now.TimeOfDay);
        }

        /// <summary>
        /// Extracts any files in the <see cref="ZipFilePath"/> to the <see cref="DirectoryPath"/>.
        /// </summary>
        /// <param name="zipFilePath">The zip file from which files are extracted.</param>
        /// <param name="directoryPath">The directory to which extracted entries are written.</param>
        /// <param name="overwrite">If true, the directory is overwritten.</param>
        public static void ExtractZipFiles(this ZipFilePath zipFilePath, DirectoryPath directoryPath, bool overwrite)
        {
            if (Directory.Exists(directoryPath) && !overwrite)
            {
                return;
            }
            using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string extractionPath = Path.Combine(directoryPath, Path.GetFileName(entry.FullName ?? Path.GetFileName(Path.GetTempFileName())));
                    entry.ExtractToFile(extractionPath, true);
                }
            }
        }

        /// <summary>
        /// Extracts any files in the <see cref="ZipFilePath"/> to the <see cref="DirectoryPath"/>.
        /// </summary>
        /// <param name="zipFilePath">The zip file from which files are extracted.</param>
        /// <param name="directoryPath">The directory to which extracted entries are written.</param>
        /// <param name="overwrite">If true, the directory is overwritten.</param>
        /// <param name="completedMessage">The message written to STDOUT upon successful completion.</param>
        public static void ExtractZipFiles(this ZipFilePath zipFilePath, DirectoryPath directoryPath, bool overwrite, string completedMessage)
        {
            zipFilePath.ExtractZipFiles(directoryPath, overwrite);
            Console.WriteLine(completedMessage, DateTime.Now.TimeOfDay);
        }
    }
}
