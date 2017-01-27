using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Represents a URL. Exceptions are thrown if the URL is invalid.
    /// </summary>
    [PublicAPI]
    public struct UrlPath : IPath
    {
        /// <summary>
        /// The string representation of the URL path.
        /// </summary>
        private readonly string _path;

        /// <summary>
        /// The Uri representation of the URL path.
        /// </summary>
        public Uri UriPath { get; }

        /// <summary>
        /// The file path extension.
        /// </summary>
        public string Extension { get; }

        /// <summary>
        /// The file name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates a new UrlPath object to hold the path to a url.
        /// </summary>
        /// <param name="urlPath">A string url path.</param>
        /// <exception cref="UriFormatException"/>
        public UrlPath(string urlPath)
        {
            if (!Uri.IsWellFormedUriString(urlPath, UriKind.RelativeOrAbsolute))
            {
                throw new UriFormatException($"Invalid URL path detected: {urlPath}");
            }
            _path = urlPath;
            UriPath = new Uri(urlPath);
            Extension = null;
            Name = null;
        }

        /// <summary>
        /// Creates a url along the path if one does not exist.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public static UrlPath Create(string urlPath)
        {
            return new UrlPath(urlPath);
        }


        /// <summary>
        /// Explicit IPath implementation.
        /// </summary>
        IPath IPath.Create(string path)
        {
            return Create(path);
        }

        /// <summary>
        /// Returns the url path.
        /// </summary>
        public override string ToString()
        {
            return _path;
        }

        IEnumerator<char> IEnumerable<char>.GetEnumerator()
        {
            return _path.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _path.AsEnumerable().GetEnumerator();
        }

        /// <summary>
        /// Implicitly casts a UrlPath as its internal string representation.
        /// </summary>
        public static implicit operator string(UrlPath urlPath)
        {
            return urlPath._path;
        }

        /// <summary>
        /// Implicitly casts a string as a UrlPath. An exception is thrown if the url is not well-formatted.
        /// </summary>
        /// <exception cref="UriFormatException"/>
        public static implicit operator UrlPath(string urlPath)
        {
            return new UrlPath(urlPath);
        }

        /// <summary>
        /// Implicitly casts a UrlPath as its internal Uri representation.
        /// </summary>
        public static implicit operator Uri(UrlPath urlPath)
        {
            return urlPath.UriPath;
        }

        /// <summary>
        /// Implicitly casts a UrlPath as a Uri. An exception is thrown if the url is not a file path URI.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        public static explicit operator FilePath(UrlPath urlPath)
        {
            if (!urlPath.UriPath.IsFile)
            {
                throw new FileNotFoundException("The specified urlPath is not a file URI.");
            }
            return new FilePath(urlPath);
        }

        /// <summary>
        /// Implicitly casts a UrlPath as a Uri. An exception is thrown if the url is not a file path URI.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public static explicit operator ZipFilePath(UrlPath urlPath)
        {
            if (!urlPath.UriPath.IsFile)
            {
                throw new FileNotFoundException("The specified urlPath is not a file URI.");
            }
            return new ZipFilePath(urlPath);
        }
    }
}