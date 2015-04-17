using System;
using System.Collections.Generic;
using System.Linq;
using Impostor.Settings;
using JetBrains.Annotations;

namespace Impostor {
    public class ImpostorSettings {
        public ImpostorSettings() {
            Rules = new List<ImpostorRule>();
        }

        public string RecordDirectoryPath { get; set; }
        [NotNull, ItemNotNull]
        public IList<ImpostorRule> Rules { get; private set; }
    }
}
