using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Impostor.Settings;
using JetBrains.Annotations;
using Microsoft.Owin;

namespace Impostor.Support {
    public class MessageSerializer {
        [NotNull]
        public async Task SerializeRequestAsync([NotNull] TextWriter writer, [NotNull] IOwinRequest request) {
            if (writer == null) throw new ArgumentNullException("writer");
            if (request == null) throw new ArgumentNullException("request");
            
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

        [NotNull]
        public async Task<Action<IOwinResponse>> DeserializeResponseAsync([NotNull] TextReader reader) {
            if (reader == null) throw new ArgumentNullException("reader");
            var result = (Action<IOwinResponse>)delegate {};

            var text = await reader.ReadToEndAsync();
            var parts = Regex.Match(text, @"^
                (?<status>[^\r\n]+)
                (?:
                    (?:\r\n|\r|\n){2}
                    (?<headers>.*)
                    (?:
                        (?:\r\n|\r|\n){2}
                        (?<body>.*)
                    )?
                )?                
            $", RegexOptions.IgnorePatternWhitespace);

            if (!parts.Success) {
                throw new ImpostorSettingsException(string.Format(
                    "Could not parse the response file. Expected format:{0}200 OK{0}{0}Header1: Value1{0}Header2: Value2{0}{0}Body",
                    Environment.NewLine
                ));
            }

            var status = parts.Groups["status"].Value.Split(' ');
            var statusCode = int.Parse(status[0]);
            var reason = status.ElementAtOrDefault(1);

            result += r => r.StatusCode = statusCode;
            if (reason != null)
                result += r => r.ReasonPhrase = reason;

            var headerGroup = parts.Groups["headers"];
            if (headerGroup.Success) {
                var headerLines = Regex.Split(headerGroup.Value, @"\r\n|\r|\n");
                foreach (var line in headerLines) {
                    var header = line.Split(':');
                    result += r => r.Headers.Append(header[0].Trim(), header[1].Trim());
                }
            }

            var bodyGroup = parts.Groups["body"];
            if (bodyGroup.Success) {
                result += r => {
                    var bytes = Encoding.UTF8.GetBytes(bodyGroup.Value);
                    r.ContentLength = bytes.Length;
                    r.Body = new MemoryStream(bytes);
                };
            }

            return result;
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