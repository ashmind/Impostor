using System;
using System.Collections.Generic;
using System.Linq;
using Impostor.Logging;
using JetBrains.Annotations;

namespace Impostor.Support {
    public class ImpostorDependencies {
        public ImpostorDependencies(
            [CanBeNull] IIOFactory ioFactory = null,
            [CanBeNull] MessageSerializer messageSerializer = null,
            [CanBeNull] RuleMatcher ruleMatcher = null,
            [CanBeNull] Func<IIOFactory, MessageSerializer, ResponseHandler> responseHandlerFactory = null,
            [CanBeNull] Func<Type, ILog> loggerFactory = null
        ) {
            ioFactory = ioFactory ?? new IOFactory();
            messageSerializer = messageSerializer ?? new MessageSerializer();
            ruleMatcher = ruleMatcher ?? new RuleMatcher();
            loggerFactory = loggerFactory ?? LogProvider.GetLogger;

            var responseHandler = (responseHandlerFactory ?? ((x1, x2) => new ResponseHandler(x1, x2)))(ioFactory, messageSerializer);

            IOFactory = ioFactory;
            MessageSerializer = messageSerializer;
            RuleMatcher = ruleMatcher;
            ResponseHandler = responseHandler;
            LoggerFactory = loggerFactory;
        }

        [NotNull] public IIOFactory IOFactory { get; private set; }
        [NotNull] public MessageSerializer MessageSerializer { get; private set; }
        [NotNull] public RuleMatcher RuleMatcher { get; private set; }
        [NotNull] public ResponseHandler ResponseHandler { get; private set; }
        [NotNull] public Func<Type, ILog> LoggerFactory { get; private set; }
    }
}
