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
        private static readonly IReadOnlyDictionary<string, string> NameMap = new Dictionary<string, string> {
            { Info.PropertyOf<ImpostorSettings>(s => s.RequestLogPath).Name, "request_log" },
            { Info.PropertyOf<ImpostorSettings>(s => s.Rules).Name,          "rules" },
            { Info.PropertyOf<Rule>(r => r.RequestUrlPath).Name,             "url" },
            { Info.PropertyOf<Rule>(r => r.ResponsePath).Name,               "response_path" },
            { Info.PropertyOf<Rule>(r => r.Response).Name,                   "response" },
            { Info.PropertyOf<RuleResponse>(r => r.StatusCode).Name,         "status" },
            { Info.PropertyOf<RuleResponse>(r => r.ContentType).Name,        "type" },
            { Info.PropertyOf<RuleResponse>(r => r.ContentPath).Name,        "body_path" },
        };
            
        [NotNull]
        public ImpostorSettings Parse([NotNull] string settings) {
            if (settings == null) throw new ArgumentNullException("settings");
            var deserializer = new Deserializer(namingConvention: new MapNamingConvention(NameMap));
            return deserializer.Deserialize<ImpostorSettings>(new StringReader(settings));
        }
    }
}
