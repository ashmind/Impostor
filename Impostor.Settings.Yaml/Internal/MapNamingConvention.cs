using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using YamlDotNet.Serialization;

namespace Impostor.Settings.Yaml.Internal {
    public class MapNamingConvention : INamingConvention {
        public MapNamingConvention([NotNull] IReadOnlyDictionary<string, string> map) {
            if (map == null) throw new ArgumentNullException("map");
            Map = map;
        }

        [NotNull] public IReadOnlyDictionary<string, string> Map { get; private set; }

        public string Apply(string value) {
            return Map[value];
        }
    }
}
