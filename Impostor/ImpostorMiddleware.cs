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
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly RequestRecorder _recorder;
        private readonly RuleMatcher _matcher;
        private readonly ImpostorSettings _settings;

        public ImpostorMiddleware(
            OwinMiddleware next,
            [NotNull] RequestRecorder recorder,
            [NotNull] RuleMatcher matcher,
            [NotNull] ImpostorSettings settings
        ) : base(next) {
            if (settings == null) throw new NullReferenceException("settings");
            if (recorder == null) throw new NullReferenceException("recorder");
            if (matcher == null) throw new NullReferenceException("matcher");
            _recorder = recorder;
            _matcher = matcher;
            _settings = settings;
        }

        [NotNull]
        public override async Task Invoke([NotNull] IOwinContext context) {
            if (context == null) throw new NullReferenceException("context");

            var request = context.Request;
            Logger.InfoFormat("{0:l} {1}", request.Method, request.Uri);

            var requestFileName = DateTime.Now.Ticks + "-" + Guid.NewGuid() + ".txt";
            var requestFilePath = Path.Combine(_settings.RecordDirectoryPath, requestFileName);
            await _recorder.RecordAsync(requestFilePath, request);

            var rule = _matcher.Match(request, _settings.Rules);
            if (rule != null) {
                Logger.DebugFormat("Request was matched by {0}.", rule);
                var response = context.Response;
                response.StatusCode = rule.StatusCode;
            }
            else {
                Logger.DebugFormat("Request was not matched by any rule.");
                await Next.Invoke(context);
            }
        }
    }
}
