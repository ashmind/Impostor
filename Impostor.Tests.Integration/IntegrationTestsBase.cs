using System;
using System.Collections.Generic;
using System.Linq;
using Impostor.Settings;
using Impostor.Support;
using Impostor.Tests.Integration.Support;
using Microsoft.Owin.Testing;
using Xunit.Abstractions;

namespace Impostor.Tests.Integration {
    public abstract class IntegrationTestsBase : IDisposable {
        private readonly ITestOutputHelper _output;
        private readonly IList<TestServer> _servers = new List<TestServer>();

        protected IntegrationTestsBase(ITestOutputHelper output) {
            _output = output;
            IOFactory = new MemoryIOFactory();
        }

        protected TestServer CreateServer(params Rule[] rules) {
            var settings = new ImpostorSettings();
            foreach (var rule in rules) {
                settings.Rules.Add(rule);
            }
            return CreateServer(settings);
        }

        protected TestServer CreateServer(ImpostorSettings settings) {
            var server = TestServer.Create(app => {
                app.UseImpostor(settings, new ImpostorDependencies(
                    // ReSharper disable once RedundantArgumentName
                    ioFactory: IOFactory,
                    loggerFactory: t => new TestOutputLogger(_output, t.Name)
                ));
            });
            _servers.Add(server);
            return server;
        }

        protected MemoryIOFactory IOFactory { get; private set; }

        public virtual void Dispose() {
            foreach (var server in _servers) {
                server.Dispose();
            }
        }
    }
}