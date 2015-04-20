using System;
using System.Collections.Generic;
using System.Linq;
using Impostor.Logging;
using JetBrains.Annotations;

namespace Impostor.Support {
    public class ImpostorDependencies {
        // TODO: Change all dependencies to interfaces
        public ImpostorDependencies(
            [CanBeNull] IFileSystem fileSystem = null,
            [CanBeNull] MessageSerializer messageSerializer = null,
            [CanBeNull] RequestMatcher requestMatcher = null,
            [CanBeNull] Func<IFileSystem, MessageSerializer, ResponseHandler> responseHandlerFactory = null,
            [CanBeNull] VariableInterpolator variableInterpolator = null,
            [CanBeNull] Func<Type, ILog> loggerFactory = null
        ) {
            fileSystem = fileSystem ?? new FileSystem();
            messageSerializer = messageSerializer ?? new MessageSerializer();
            requestMatcher = requestMatcher ?? new RequestMatcher();
            variableInterpolator = variableInterpolator ?? new VariableInterpolator();
            loggerFactory = loggerFactory ?? LogProvider.GetLogger;

            var responseHandler = (responseHandlerFactory ?? ((x1, x2) => new ResponseHandler(x1, x2)))(fileSystem, messageSerializer);

            FileSystem = fileSystem;
            MessageSerializer = messageSerializer;
            RequestMatcher = requestMatcher;
            ResponseHandler = responseHandler;
            VariableInterpolator = variableInterpolator;
            LoggerFactory = loggerFactory;
        }

        [NotNull] public IFileSystem FileSystem { get; private set; }
        [NotNull] public MessageSerializer MessageSerializer { get; private set; }
        [NotNull] public RequestMatcher RequestMatcher { get; private set; }
        [NotNull] public VariableInterpolator VariableInterpolator { get; set; }
        [NotNull] public ResponseHandler ResponseHandler { get; private set; }
        [NotNull] public Func<Type, ILog> LoggerFactory { get; private set; }
    }
}
