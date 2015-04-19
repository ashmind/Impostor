using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Impostor.Settings;
using JetBrains.Annotations;
using Microsoft.Owin;

namespace Impostor.Support {
    public class ResponseHandler {
        private readonly MessageSerializer _serializer;
        private readonly IIOFactory _ioFactory;

        public ResponseHandler([NotNull] IIOFactory ioFactory, [NotNull] MessageSerializer serializer) {
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (ioFactory == null) throw new ArgumentNullException("ioFactory");
            _serializer = serializer;
            _ioFactory = ioFactory;
        }

        public async Task ProcessResponseAsync([NotNull] IOwinResponse response, [NotNull] Rule rule) {
            if (response == null) throw new ArgumentNullException("response");
            if (rule == null) throw new ArgumentNullException("rule");

            if (!string.IsNullOrEmpty(rule.ResponsePath)) {
                if (rule.Response != null)
                    throw new ImpostorSettingsException(string.Format("Rule must not have both response and response path set at the same time (rule '{0}').", rule.RequestUrlPath));

                var responseAction = await ReadResponseAsync(rule.ResponsePath);
                responseAction(response);
                return;
            }

            if (rule.Response == null)
                throw new ImpostorSettingsException(string.Format("Rule must have either response or response path set (rule '{0}').", rule.RequestUrlPath));

            response.StatusCode = rule.Response.StatusCode;
            if (!string.IsNullOrEmpty(rule.Response.ContentType))
                response.ContentType = rule.Response.ContentType;

            if (string.IsNullOrEmpty(rule.Response.ContentPath))
                return;

            using (var bodyStream = _ioFactory.CreateReadStream(rule.Response.ContentPath)) {
                var bytes = new byte[bodyStream.Length];
                await bodyStream.ReadAsync(bytes, 0, bytes.Length);

                response.ContentLength = bytes.Length;
                response.Body = new MemoryStream(bytes);
            }
        }

        private async Task<Action<IOwinResponse>> ReadResponseAsync(string responsePath) {
            using (var reader = _ioFactory.CreateTextReader(responsePath)) {
                return await _serializer.DeserializeResponseAsync(reader);
            }
        }
    }
}
