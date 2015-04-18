using YamlDotNet.Serialization;

namespace Impostor.Settings.Yaml {
    internal class RuleYaml {
        public RuleYaml() {
        }

        [YamlMember(Alias = "url")]
        public string ResponsePath { get; set; }

        [YamlMember(Alias = "status")]
        public int StatusCode { get; set; }

        public ImpostorRule ToRule() {
            return new ImpostorRule {
                ResponsePath = ResponsePath,
                StatusCode = StatusCode
            };
        }
    }
}