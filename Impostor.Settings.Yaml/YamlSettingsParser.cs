using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Impostor.Settings.Yaml.Internal;
using InfoOf;
using JetBrains.Annotations;
using YamlDotNet.Serialization;

namespace Impostor.Settings.Yaml {
    public class YamlSettingsParser {
        private readonly IReadOnlyDictionary<string, string> NameMap = new Dictionary<string, string> {
            { Info.PropertyOf<ImpostorSettings>(s => s.RequestLogPath).Name, "request_log" },
            { Info.PropertyOf<ImpostorSettings>(s => s.Rules).Name,         "rules" },
            { Info.PropertyOf<ImpostorRule>(r => r.RequestUrlPath).Name,    "url" },
            { Info.PropertyOf<ImpostorRule>(r => r.StatusCode).Name,        "status" },
            { Info.PropertyOf<ImpostorRule>(r => r.ResponsePath).Name,      "response" }
        };
            
        [NotNull]
        public ImpostorSettings Parse([NotNull] string settings) {
            if (settings == null) throw new ArgumentNullException("settings");
            var deserializer = new Deserializer(namingConvention: new MapNamingConvention(NameMap));
            return deserializer.Deserialize<ImpostorSettings>(new StringReader(settings));
        }
    }
}
