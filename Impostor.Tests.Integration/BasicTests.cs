using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Impostor.Settings;
using Xunit;
using Xunit.Abstractions;

namespace Impostor.Tests.Integration {
    public class BasicTests : IntegrationTestsBase {
        public BasicTests(ITestOutputHelper output) : base(output) {
        }

        [Fact]
        public async Task Request_ReturnsStatusCodeFromRule_IfMatchedByUrlPath() {
            var server = CreateServer(new ImpostorSettings { Rules = {
                new Rule { RequestUrlPath = "/test", Response = new RuleResponse { StatusCode = 222 } }
            }});

            var response = await server
                .CreateRequest("/test")
                .GetAsync();

            Assert.Equal(222, (int)response.StatusCode);
        }

        [Fact]
        public async Task Request_ReturnsStatusCodeFromRuleResponseFile_IfMatchedByUrlPath() {
            Directory.CreateDirectory("Responses");
            var path = "Responses\\" + Guid.NewGuid() + ".txt";
            File.WriteAllText(path, "222 OK");

            var server = CreateServer(new ImpostorSettings { Rules = {
                new Rule { RequestUrlPath = "/test", ResponsePath = path }
            }});

            var response = await server
                .CreateRequest("/test")
                .GetAsync();

            Assert.Equal(222, (int)response.StatusCode);
        }
    }
}
