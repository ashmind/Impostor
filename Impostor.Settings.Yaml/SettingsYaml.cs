using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace Impostor.Settings.Yaml {
    internal class SettingsYaml {
        public SettingsYaml() {
            Rules = new List<RuleYaml>();
        }

        [YamlMember(Alias = "request_log")]
        public string RequestLogPath { get; set; }

        [YamlMember(Alias = "rules")]
        public IList<RuleYaml> Rules { get; set; }

        public ImpostorSettings ToSettings() {
            var settings = new ImpostorSettings {
                RequestLogPath = RequestLogPath
            };
            foreach (var rule in Rules) {
                settings.Rules.Add(rule.ToRule());
            }
            return settings;
        }
    }
}
