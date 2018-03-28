using System;
using System.IO;
using System.Net.Http;
using AD.IO.Paths;
using JetBrains.Annotations;

namespace AD.IO.Compression
{
    /// <summary>
    /// Extensions methods to download zip files from a <see cref="UrlPath"/>.
    /// </summary>
    [PublicAPI]
    public static class GetZipFileExtensions
    {
        /// <summary>
        /// Saves a zip file to <paramref name="zipFilePath"/> from the response stream of the <paramref name="urlPath"/>.
        /// </summary>
        /// <param name="urlPath">The address from which the zip file is returned.</param>
        /// <param name="zipFilePath">The file path to which the zip file is saved.</param>
        /// <param name="overwrite">If true, the zip file is overwritten.</param>
        public static void GetZipFile(this UrlPath urlPath, ZipFilePath zipFilePath, bool overwrite)
        {
            if (File.Exists(zipFilePath) && !overwrite)
            {
                return;
            }
            HttpClient client = new HttpClient
            {
                Timeout = new TimeSpan(1, 0, 0)
            };
            using (Stream stream = client.GetStreamAsync(urlPath.UriPath).Result)
            {
                if (stream is null)
                {
                    throw new NullReferenceException("HttpClient returned a null stream to GetZipFile(UrlPath path)");
                }

                using (FileStream fileStream = new FileStream(zipFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }

        /// <summary>
        /// Tries to save a response stream to the <see cref="ZipFilePath"/>. This method continues until sucessful.
        /// </summary>
        /// <param name="urlPath">The address from which the zip file is returned.</param>
        /// <param name="zipFilePath">The file path to which the zip file is saved.</param>
        /// <param name="overwrite">If true, the zip file is overwritten.</param>
        /// <param name="completedMessage">A message written to stdout upon completion.</param>
        public static void TryGetZipFile(this UrlPath urlPath, ZipFilePath zipFilePath, bool overwrite, string completedMessage)
        {
            while (true)
            {
                try
                {
                    urlPath.GetZipFile(zipFilePath, overwrite);
                    Console.WriteLine(completedMessage, DateTime.Now.TimeOfDay);
                    Console.Beep();
                    return;
                }
                catch
                {
                    Console.Error.WriteLine(@">> An exception occured while downloading the zip file. Retrying...");
                }
            }
        }
    }
}
