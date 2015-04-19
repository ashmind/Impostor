using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Impostor.Support;

namespace Impostor.Tests.Integration.Support {
    public class MemoryIOFactory : IIOFactory {
        public MemoryIOFactory() {
            Streams = new Dictionary<string, MemoryStream>();
        }

        public IDictionary<string, MemoryStream> Streams { get; private set; }

        public void SetAllText(string path, string text) {
            Streams[path] = new MemoryStream(Encoding.UTF8.GetBytes(text));
        }
        
        public TextWriter CreateTextWriter(string path) {
            var stream = GetStreamOrNull(path);
            if (stream == null) {
                stream = new MemoryStream();
                Streams[path] = stream;
            }

            return new StreamWriter(Streams[path]);
        }

        public TextReader CreateTextReader(string path) {
            return new StreamReader(CreateReadStream(path));
        }

        public Stream CreateReadStream(string path) {
            var stream = GetStreamOrNull(path);
            if (stream == null)
                throw new FileNotFoundException("Path '" + path + "' was no registered in MemoryIOFactory.");

            return stream;
        }

        private MemoryStream GetStreamOrNull(string path) {
            MemoryStream stream;
            if (!Streams.TryGetValue(path, out stream))
                return null;

            return stream;
        }
    }
}
