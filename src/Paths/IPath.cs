using System.Collections.Generic;
using JetBrains.Annotations;

namespace AD.IO
{
    [PublicAPI]
    public interface IPath : IEnumerable<char>
    {
        string Extension { get; }

        string Name { get; }

        IPath Create(string path);

        string ToString();
    }
}