using System.IO;
using JetBrains.Annotations;

namespace Impostor.Support {
    public interface IFileSystem {
        void EnsureDirectory([NotNull] string path);
        [NotNull] TextWriter CreateTextWriter([NotNull] string path);
        [NotNull] TextReader CreateTextReader([NotNull] string path);
        [NotNull] Stream CreateReadStream([NotNull] string path);
    }
}