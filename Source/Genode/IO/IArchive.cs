using System;
using System.Collections.Generic;
using System.Text;

namespace Genode.IO
{
    /// <summary>
    /// Represents a collection of Raw Contents.
    /// </summary>
    public interface IArchive<T, P> : IDisposable
        where P : IContentProcessor<T>
    {
        string GetIdentifier(int index);

        string[] GetIdentifiers();

        T GetContent(int index);

        T GetContent(string identifier);

        IDictionary<string, T> GetContents();
    }
}
