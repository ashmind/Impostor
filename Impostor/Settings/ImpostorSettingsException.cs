using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Impostor.Settings {
    [Serializable]
    public class ImpostorSettingsException : Exception {
        public ImpostorSettingsException() {}
        public ImpostorSettingsException(string message) : base(message) {}
        public ImpostorSettingsException(string message, Exception inner) : base(message, inner) {}
        protected ImpostorSettingsException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}
