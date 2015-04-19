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

        public ResponseHandler([NotNull] MessageSerializer serializer) {
            if (serializer == null) throw new ArgumentNullException("serializer");
            _serializer = serializer;
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

            var responseBytes = File.ReadAllBytes(rule.Response.ContentPath);
            response.ContentLength = responseBytes.Length;
            response.Body = new MemoryStream(responseBytes);
        }

        private async Task<Action<IOwinResponse>> ReadResponseAsync(string responsePath) {
            using (var reader = File.OpenText(responsePath)) {
                return await _serializer.DeserializeResponseAsync(reader);
            }
        }
    }
}
