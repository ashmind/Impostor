using System;
using System.Collections.Generic;
using System.Linq;
using Impostor.Logging;
using JetBrains.Annotations;

namespace Impostor.Support {
    public class ImpostorDependencies {
        public ImpostorDependencies(
            [CanBeNull] MessageSerializer messageSerializer = null,
            [CanBeNull] RuleMatcher ruleMatcher = null,
            [CanBeNull] Func<MessageSerializer, ResponseHandler> responseHandlerFactory = null,
            [CanBeNull] Func<Type, ILog> loggerFactory = null
        ) {
            messageSerializer = messageSerializer ?? new MessageSerializer();
            ruleMatcher = ruleMatcher ?? new RuleMatcher();
            loggerFactory = loggerFactory ?? LogProvider.GetLogger;

            var responseHandler = (responseHandlerFactory ?? (s => new ResponseHandler(s)))(messageSerializer);

            MessageSerializer = messageSerializer;
            RuleMatcher = ruleMatcher;
            ResponseHandler = responseHandler;
            LoggerFactory = loggerFactory;
        }

        [NotNull] public MessageSerializer MessageSerializer { get; private set; }
        [NotNull] public RuleMatcher RuleMatcher { get; private set; }
        [NotNull] public ResponseHandler ResponseHandler { get; private set; }
        [NotNull] public Func<Type, ILog> LoggerFactory { get; private set; }
    }
}
