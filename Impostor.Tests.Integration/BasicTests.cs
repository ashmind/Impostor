using System;
using System.Collections.Generic;
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
        public async Task Request_LogPathVariablesAreInterpolated() {
            var year = DateTime.Now.Year.ToString();
            var server = CreateServer(new ImpostorSettings {
                RequestLogPath = "{$now:yyyy}.txt"
            });
            await server.CreateRequest("/").GetAsync();

            Assert.True(FileSystem.Streams.ContainsKey(year + ".txt"));
        }

        [Fact]
        public async Task Request_ReturnsStatusCodeFromRule_IfMatchedByUrlPath() {
            var server = CreateServer(
                new Rule { RequestUrlPath = "/test", Response = new RuleResponse { StatusCode = 222 } }
            );
            var response = await server.CreateRequest("/test").GetAsync();

            Assert.Equal(222, (int)response.StatusCode);
        }

        [Fact]
        public async Task Request_ReturnsStatusCodeFromRuleResponseFile_IfMatchedByUrlPath() {
            var server = CreateServer(CreateRuleWithResponseFile("/test", "222 OK"));
            var response = await server.CreateRequest("/test").GetAsync();

            Assert.Equal(222, (int)response.StatusCode);
        }

        [Fact]
        public async Task Request_ReturnsHeaderFromRuleResponseFile_IfMatchedByUrlPath() {
            var server = CreateServer(
                CreateRuleWithResponseFile("/test", "200 OK\r\n\r\nX-Test: ABC")
            );
            var response = await server.CreateRequest("/test").GetAsync();

            Assert.Equal("ABC", response.Headers.GetValues("X-Test").SingleOrDefault());
        }

        private Rule CreateRuleWithResponseFile(string urlPath, string responseFileText) {
            FileSystem.SetAllText("response.txt", responseFileText);
            return new Rule { RequestUrlPath = urlPath, ResponsePath = "response.txt" };
        }
    }
}
