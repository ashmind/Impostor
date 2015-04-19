using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Impostor.Settings {
    [PublicAPI]
    public class RuleResponse {
        public RuleResponse() {
            StatusCode = 200;
        }
        public int StatusCode { get; set; }
        public string ContentType { get; set; }
        public string ContentPath { get; set; }
    }
}
