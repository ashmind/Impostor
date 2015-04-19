using System;
using System.Collections.Generic;
using System.Linq;
using Impostor.Settings;
using JetBrains.Annotations;

namespace Impostor {
    [PublicAPI]
    public class ImpostorSettings {
        public ImpostorSettings() {
            Rules = new List<Rule>();
        }

        [CanBeNull] public string RequestLogPath { get; set; }
        [NotNull, ItemNotNull]
        public IList<Rule> Rules { get; private set; }
    }
}
