using System;
using System.Collections.Generic;
using System.Linq;
using Impostor.Logging;
using Xunit.Abstractions;

namespace Impostor.Tests.Integration.Support {
    public class TestOutputLogger : ILog {
        private readonly ITestOutputHelper _output;
        private readonly string _name;

        public TestOutputLogger(ITestOutputHelper output, string name) {
            _output = output;
            _name = name;
        }

        public bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters) {
            if (messageFunc == null && exception == null)
                return true;

            var message = (messageFunc != null) ? string.Format(messageFunc(), formatParameters) : "";
            _output.WriteLine("[{0}] [{1}] {2}{3}{4}", logLevel, _name, message, exception != null ? Environment.NewLine : null, exception);
            return true;
        }
    }
}
