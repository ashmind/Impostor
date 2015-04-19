using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Impostor.Settings.Yaml;
using Newtonsoft.Json;
using Xunit;

namespace Impostor.Tests.Unit.Of.Settings.Yaml {
    public class YamlSettingsParserTests {
        [Theory]
        [InlineData(
            "request_log: ./{ticks}-{guid}.txt",
            "{RequestLogPath:'./{ticks}-{guid}.txt',Rules:[]}"
        )]
        [InlineData(
            "rules:\r\n  - url: /test\r\n    response:\r\n      status: 200",
            "{Rules:[{RequestUrlPath:'/test',Response:{StatusCode:200}}]}"
        )]
        public void Parse_ReturnsExpectedSettings(string settingsString, string expectedLiteJson) {
            var parsed = new YamlSettingsParser().Parse(settingsString);
            Assert.Equal(expectedLiteJson, ToLiteJson(parsed));
        }

        private string ToLiteJson(object value) {
            var stringWriter = new StringWriter();
            using (var jsonWriter = new JsonTextWriter(stringWriter)) {
                jsonWriter.QuoteName = false;
                jsonWriter.QuoteChar = '\'';
                new JsonSerializer {
                    NullValueHandling = NullValueHandling.Ignore
                }.Serialize(jsonWriter, value);
            }
            return stringWriter.ToString();
        }
    }
}
