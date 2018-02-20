using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Genode;

namespace CXO2.Processors
{
    public class ChartProcessorFactory
    {
        private static Dictionary<Type, Func<string, ChartDecoder>> decoders;

        static ChartProcessorFactory()
        {
            decoders = new Dictionary<Type, Func<string, ChartDecoder>>();

            // Register Built-in readers
            Install<O2Jam.O2ChartDecoder>((filename) => new O2Jam.O2ChartDecoder(filename));
        }

        /// <summary>
        /// Check whether the specified <see cref="ChartDecoder"/> is already registered.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="ChartDecoder"/> to check.</typeparam>
        /// <returns><code>true</code> if registered, otherwise false.</returns>
        public static bool IsInstalled<T>()
            where T : ChartDecoder
        {
            return decoders.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Register specified <see cref="ChartDecoder"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="ChartDecoder"/> to register.</typeparam>
        public static void Install<T>(Func<string, T> factory)
            where T : ChartDecoder
        {
            if (IsInstalled<T>())
            {
                Logger.Warning("{0} is already registered.", typeof(T).Name);
                return;
            }

            decoders.Add(typeof(T), factory);
        }

        /// <summary>
        /// Unregister specified <see cref="ChartDecoder"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="ChartDecoder"/> to unregister.</typeparam>
        public static void Uninstall<T>()
            where T : ChartDecoder
        {
            if (!IsInstalled<T>())
            {
                Logger.Warning("{0} is not registered.", typeof(T).Name);
                return;
            }

            decoders.Remove(typeof(T));
        }

        /// <summary>
        /// Create a registered instance of <see cref="ChartDecoder"/> from specified filename.
        /// </summary>
        /// <param name="filename">Path of <see cref="File"/> that contains chart data.</param>
        /// <returns><see cref="ChartDecoder"/> that can handle the data, otherwise null.</returns>
        public static ChartDecoder GetDecoder(string filename)
        {
            foreach (var decoder in decoders)
            {
                var instance = decoder.Value(filename);
                if (!instance.Invalid)
                    return instance;
            }

            return null;
        }
    }
}
