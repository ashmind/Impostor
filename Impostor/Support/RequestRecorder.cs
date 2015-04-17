using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Owin;

namespace Impostor.Support {
    public class RequestRecorder {
        public async Task RecordAsync([NotNull] string filePath, [NotNull] IOwinRequest request) {
            if (filePath == null) throw new ArgumentNullException("filePath");
            if (request == null) throw new ArgumentNullException("request");

            using (var writer = new StreamWriter(filePath)) {
                await writer.WriteLineAsync(request.Method + " " + request.Uri);
                await writer.WriteLineAsync();

                var maxHeaderWidth = request.Headers.Max(h => h.Key.Length);
                foreach (var header in request.Headers) {
                    foreach (var value in header.Value) {
                        var line = string.Format("{0} {1}", (header.Key + ":").PadRight(maxHeaderWidth + 1), value);
                        await writer.WriteLineAsync(line);
                    }
                }
                await writer.WriteLineAsync();

                var bodyEncoding = GetContentEncoding(request) ?? Encoding.UTF8;
                using (var reader = new StreamReader(request.Body, bodyEncoding)) {
                    var block = new char[1024];
                    var readLength = await reader.ReadBlockAsync(block, 0, block.Length);
                    while (readLength > 0) {
                        await writer.WriteAsync(block, 0, readLength);
                        readLength = await reader.ReadBlockAsync(block, 0, block.Length);
                    }
                }
            }
        }

        private static Encoding GetContentEncoding(IOwinRequest request) {
            if (string.IsNullOrEmpty(request.ContentType))
                return null;

            ContentType contentType;
            try {
                contentType = new ContentType(request.ContentType);
            }
            catch (FormatException) {
                return null;
            }

            try {
                return Encoding.GetEncoding(contentType.CharSet);
            }
            catch (ArgumentException) {
                return null;
            }
        }
    }
}
