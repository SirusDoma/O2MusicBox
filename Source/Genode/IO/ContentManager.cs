using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Genode.IO
{
    public sealed class ContentManager
    {
        private static readonly ContentManager _instance = new ContentManager();

        /// <summary>
        /// Gets the singleton instance of <see cref="ContentManager"/>.
        /// </summary>
        public static ContentManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private Dictionary<string, object> _contents;
        private Dictionary<object, object> _processors;

        private ContentManager()
        {
            _contents    = new Dictionary<string, object>();
            _processors  = new Dictionary<object, object>();
        }

        public void InstallProcessor<T, V>(V parser)
            where V : IContentProcessor<T>, new ()
        {
            if (!_processors.ContainsKey(typeof(T)))
            {
                _processors.Add(typeof(T), parser);
            }
        }

        public T Load<T>(string filename)
            where T : new()
        {
            if (!_processors.ContainsKey(typeof(T)))
            {
                throw new Exception("Unknown type of content.\nMake sure the IContentParser is registered.");
            }

            var parser = _processors[typeof(T)] as IContentProcessor<T>;
            var content = parser.Parse(new FileStream(filename, FileMode.Open));
            var fi = new FileInfo(filename);

            if (_contents.ContainsKey(fi.Name))
            {
                var disposable = _contents[fi.Name] as IDisposable;
                disposable?.Dispose();
            }

            _contents[fi.Name] = content;
            return content;
        }

        public T Load<T>(string name, Stream stream)
            where T : new()
        {
            if (!_processors.ContainsKey(typeof(T)))
            {
                throw new Exception("Unknown type of content.\nMake sure the IContentParser is registered.");
            }

            var parser = _processors[typeof(T)] as IContentProcessor<T>;
            var content = parser.Parse(stream);

            _contents[name] = content;
            return content;
        }

        public T GetContent<T>(string name)
        {
            return (T)_contents[name];
        }

        public void Clear()
        {
            foreach (var content in _contents.Values)
            {
                var disposable = content as IDisposable;
                disposable?.Dispose();
            }

            _contents.Clear();
        }
    }
}
