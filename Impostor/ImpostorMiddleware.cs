using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Impostor.Logging;
using Impostor.Support;
using JetBrains.Annotations;
using Microsoft.Owin;

namespace Impostor {
    public class ImpostorMiddleware : OwinMiddleware {
        private readonly MessageSerializer _serializer;
        private readonly RuleMatcher _matcher;
        private readonly ResponseHandler _responseHandler;
        private readonly ImpostorSettings _settings;
        private readonly ILog _logger;

        public ImpostorMiddleware(
            OwinMiddleware next,
            [NotNull] ImpostorDependencies dependencies,
            [NotNull] ImpostorSettings settings
        ) : base(next) {
            if (settings == null) throw new NullReferenceException("settings");
            if (dependencies == null) throw new NullReferenceException("dependencies");
            _serializer = dependencies.MessageSerializer;
            _matcher = dependencies.RuleMatcher;
            _responseHandler = dependencies.ResponseHandler;
            _settings = settings;
            _logger = dependencies.LoggerFactory(GetType());
        }

        [NotNull]
        public override async Task Invoke([NotNull] IOwinContext context) {
            if (context == null) throw new NullReferenceException("context");

            var request = context.Request;
            _logger.InfoFormat("{0:l} {1}", request.Method, request.Uri);

            if (_settings.RequestLogPath != null) {
                using (var writer = new StreamWriter(_settings.RequestLogPath)) {
                    await _serializer.SerializeRequestAsync(writer, request);
                }
            }

            var rule = _matcher.Match(request, _settings.Rules);
            if (rule != null) {
                _logger.DebugFormat("Request was matched by {0}.", rule);
                await _responseHandler.ProcessResponseAsync(context.Response, rule);
            }
            else {
                _logger.DebugFormat("Request was not matched by any rule.");
                await Next.Invoke(context);
            }
        }
    }
}
