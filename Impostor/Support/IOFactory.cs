using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Impostor.Support {
    public class IOFactory : IIOFactory {
        public TextWriter CreateTextWriter(string path) {
            return new StreamWriter(path);
        }

        public TextReader CreateTextReader(string path) {
            return new StreamReader(path);
        }

        public Stream CreateReadStream(string path) {
            return File.OpenRead(path);
        }
    }
}
