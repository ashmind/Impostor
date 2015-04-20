using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Impostor.Logging;
using Impostor.Support;
using JetBrains.Annotations;
using Microsoft.Owin;

namespace Impostor {
    public class ImpostorMiddleware : OwinMiddleware {
        private readonly ImpostorDependencies _services;
        private readonly ImpostorSettings _settings;
        private readonly ILog _logger;

        public ImpostorMiddleware(
            OwinMiddleware next,
            [NotNull] ImpostorDependencies services,
            [NotNull] ImpostorSettings settings
        ) : base(next) {
            if (settings == null) throw new NullReferenceException("settings");
            if (services == null) throw new NullReferenceException("services");
            _services = services;
            _settings = settings;
            _logger = services.LoggerFactory(GetType());
        }

        [NotNull]
        public override async Task Invoke([NotNull] IOwinContext context) {
            if (context == null) throw new NullReferenceException("context");

            var request = context.Request;
            _logger.InfoFormat("{0:l} {1}", request.Method, request.Uri);

            if (_settings.RequestLogPath != null) {
                var interpolatedPath = _services.VariableInterpolator.Interpolate(_settings.RequestLogPath);
                var directoryPath = Path.GetDirectoryName(interpolatedPath);
                if (directoryPath != null)
                    _services.FileSystem.EnsureDirectory(directoryPath);

                using (var writer = _services.FileSystem.CreateTextWriter(interpolatedPath)) {
                    await _services.MessageSerializer.SerializeRequestAsync(writer, request);
                }
            }

            var rule = _services.RequestMatcher.Match(request, _settings.Rules);
            if (rule != null) {
                _logger.DebugFormat("Request was matched by {0}.", rule);
                await _services.ResponseHandler.ProcessResponseAsync(context.Response, rule);
            }
            else {
                _logger.DebugFormat("Request was not matched by any rule.");
                await Next.Invoke(context);
            }
        }
    }
}
