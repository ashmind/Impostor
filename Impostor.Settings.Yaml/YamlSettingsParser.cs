using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Impostor.Settings.Yaml.Internal;
using InfoOf;
using JetBrains.Annotations;
using YamlDotNet.RepresentationModel;
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

            //var yaml = new YamlStream();
            //yaml.Load(new StringReader(settings));

            //if (yaml.Documents.Count == 0)
            //    throw Error("Found no YAML documents.");

            //if (yaml.Documents.Count > 1)
            //    throw Error("Found more than one YAML document.");

            //var document = yaml.Documents.Single();
            //var mapping = document.RootNode as YamlMappingNode;
            //if (mapping == null)
            //    throw Error("Expected root node to be a mapping (key: value).");

            //var result = new ImpostorSettings();
            //Apply(mapping, RootMap);

            //return raw.ToSettings();
        }

        private static void Apply(YamlMappingNode mapping, IReadOnlyDictionary<string, Action<YamlNode>> appliers) {
            foreach (var pair in mapping.Children) {
                var keyScalar = pair.Key as YamlScalarNode;
                if (keyScalar == null)
                    throw Error("Expected key to be a scalar, found '{0}' ({1}).", pair.Key, pair.Key.GetType());

                Action<YamlNode> apply;
                if (!appliers.TryGetValue(keyScalar.Value, out apply))
                    throw Error("Unexpected key '{0}'.", keyScalar.Value);

                apply(pair.Value);
            }
        }

        [StringFormatMethod("message")]
        private static FormatException Error(string message, params object[] args) {
            return new FormatException(string.Format(message, args));
        }
    }
}
