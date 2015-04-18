using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Impostor.Settings {
    [Serializable]
    public class ImpostorSettingsLoadException : Exception {
        public ImpostorSettingsLoadException() {}
        public ImpostorSettingsLoadException(string message) : base(message) {}
        public ImpostorSettingsLoadException(string message, Exception inner) : base(message, inner) {}
        protected ImpostorSettingsLoadException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}
